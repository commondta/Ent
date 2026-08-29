using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace HRMS_Web.Services.ErpPlatform
{
    /// <summary>
    /// One application the signed-in user may open (ERP_Platform.dbo.Applications via role mapping).
    /// </summary>
    public sealed class ErpApplication
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string BaseUrl { get; set; } = "/";
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Gateway to the central ERP_Platform database (identity, applications, SSO sessions).
    /// The .NET PMS login is the single login for the whole solution: after it authenticates a
    /// user it creates a central session here and hands the token to the browser as the
    /// <c>erp_sso</c> cookie; the other applications (LIMS today, HRMS later) only trust that cookie.
    /// Plain ADO.NET on purpose – the platform DB is deliberately outside the PMS EF model.
    /// </summary>
    public sealed class ErpPlatformService
    {
        private readonly string _cs;
        private readonly IConfiguration _cfg;
        private readonly ILogger<ErpPlatformService> _log;

        public const string DefaultCookieName = "erp_sso";

        public ErpPlatformService(IConfiguration cfg, ILogger<ErpPlatformService> log)
        {
            _cfg = cfg; _log = log;
            _cs = cfg.GetConnectionString("ErpPlatform") ?? "";
        }

        public bool Enabled => !string.IsNullOrWhiteSpace(_cs) && _cfg.GetValue("Erp:Enabled", true);
        public string CookieName => _cfg["Erp:CookieName"] ?? DefaultCookieName;
        public int SessionHours => _cfg.GetValue("Erp:SessionHours", 9);

        /// <summary>Ensures the central user row exists (by PMS username) and opens a central session. Returns the token.</summary>
        public string CreateSession(int pmsUserId, string username, string? fullName, string? ip, string? userAgent)
        {
            var token = NewToken();
            using var con = new SqlConnection(_cs);
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
DECLARE @uid int = (SELECT Id FROM dbo.Users WHERE Username = @u);
IF @uid IS NULL BEGIN
    INSERT INTO dbo.Users (Username, Email, FullName, IsActive, PmsUserId) VALUES (@u, NULL, @n, 1, @p);
    SET @uid = SCOPE_IDENTITY();
    -- a new PMS user gets PMS access by default
    INSERT INTO dbo.UserRoles (UserId, RoleId) SELECT @uid, Id FROM dbo.Roles WHERE Code = 'PMS_USER';
END
ELSE UPDATE dbo.Users SET PmsUserId = @p, FullName = COALESCE(FullName, @n), LastLoginAt = SYSUTCDATETIME() WHERE Id = @uid;
INSERT INTO dbo.Sessions (Token, UserId, ExpiresAt, LastSeenAt, IpAddress, UserAgent, SourceApp)
VALUES (@t, @uid, DATEADD(HOUR, @h, SYSUTCDATETIME()), SYSUTCDATETIME(), @ip, @ua, 'PMS');
INSERT INTO dbo.AuditLogs (UserId, App, Event, Detail, IpAddress) VALUES (@uid, 'PMS', 'LOGIN', 'central session opened', @ip);
SELECT @uid;";
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@n", (object?)fullName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p", pmsUserId);
                cmd.Parameters.AddWithValue("@t", token);
                cmd.Parameters.AddWithValue("@h", SessionHours);
                cmd.Parameters.AddWithValue("@ip", (object?)Trunc(ip, 64) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ua", (object?)Trunc(userAgent, 300) ?? DBNull.Value);
                cmd.ExecuteScalar();
            }
            return token;
        }

        /// <summary>Central user id for a live token, or null when missing / expired / revoked.</summary>
        public int? Validate(string? token)
        {
            if (!IsToken(token)) return null;
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT s.UserId FROM dbo.Sessions s JOIN dbo.Users u ON u.Id = s.UserId
                                WHERE s.Token = @t AND s.RevokedAt IS NULL AND s.ExpiresAt > SYSUTCDATETIME() AND u.IsActive = 1";
            cmd.Parameters.AddWithValue("@t", token!);
            var o = cmd.ExecuteScalar();
            return o == null || o == DBNull.Value ? null : Convert.ToInt32(o);
        }

        public void Revoke(string? token)
        {
            if (!IsToken(token)) return;
            try
            {
                using var con = new SqlConnection(_cs);
                con.Open();
                using var cmd = con.CreateCommand();
                cmd.CommandText = @"UPDATE dbo.Sessions SET RevokedAt = SYSUTCDATETIME() WHERE Token = @t AND RevokedAt IS NULL;
                                    INSERT INTO dbo.AuditLogs (UserId, App, Event, Detail) SELECT UserId, 'PMS', 'LOGOUT', 'central session revoked' FROM dbo.Sessions WHERE Token = @t;";
                cmd.Parameters.AddWithValue("@t", token!);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { _log.LogWarning(ex, "ERP session revoke failed"); }
        }

        /// <summary>Applications the central user may open, in display order.</summary>
        public List<ErpApplication> ApplicationsFor(int erpUserId)
        {
            var list = new List<ErpApplication>();
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT a.Code, a.Name, a.Description, a.BaseUrl, a.SortOrder, a.IsActive
                                FROM dbo.UserRoles ur
                                JOIN dbo.RoleApplication ra ON ra.RoleId = ur.RoleId
                                JOIN dbo.Applications a ON a.Id = ra.ApplicationId
                                WHERE ur.UserId = @u ORDER BY a.SortOrder";
            cmd.Parameters.AddWithValue("@u", erpUserId);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new ErpApplication
                {
                    Code = r.GetString(0), Name = r.GetString(1),
                    Description = r.IsDBNull(2) ? null : r.GetString(2),
                    BaseUrl = r.IsDBNull(3) ? "/" : r.GetString(3),
                    SortOrder = r.GetInt32(4), IsActive = r.GetBoolean(5)
                });
            }
            return list;
        }

        /// <summary>Applications for a token (convenience for the header switcher / My Home).</summary>
        public List<ErpApplication> ApplicationsForToken(string? token)
        {
            var uid = Validate(token);
            return uid == null ? new List<ErpApplication>() : ApplicationsFor(uid.Value);
        }

        // ------------------------------------------------------------------
        // Central credentials: ONE login page accepts every solution's credentials.
        // Order at login: PMS account → central hash → LIMS-native account (verified by LIMS over the
        // shared secret, then stored centrally so the centre becomes the credential authority).
        // ------------------------------------------------------------------

        public sealed class CentralUser
        {
            public int Id; public string Username = ""; public string? Email; public string? FullName;
            public int? PmsUserId; public int? LimsUserId; public byte[]? PasswordHash; public byte[]? PasswordKey; public bool IsActive;
        }

        /// <summary>Central user by id (the id a live token validates to).</summary>
        public CentralUser? FindUserById(int id)
        {
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Id, Username, Email, FullName, PmsUserId, LimsUserId, PasswordHash, PasswordKey, IsActive FROM dbo.Users WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;
            return new CentralUser
            {
                Id = r.GetInt32(0), Username = r.GetString(1), Email = r.IsDBNull(2) ? null : r.GetString(2), FullName = r.IsDBNull(3) ? null : r.GetString(3),
                PmsUserId = r.IsDBNull(4) ? null : r.GetInt32(4), LimsUserId = r.IsDBNull(5) ? null : r.GetInt32(5),
                PasswordHash = r.IsDBNull(6) ? null : (byte[])r[6], PasswordKey = r.IsDBNull(7) ? null : (byte[])r[7], IsActive = r.GetBoolean(8)
            };
        }

        public CentralUser? FindUser(string username)
        {
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 Id, Username, Email, FullName, PmsUserId, LimsUserId, PasswordHash, PasswordKey, IsActive FROM dbo.Users WHERE Username = @u OR Email = @u ORDER BY CASE WHEN Username = @u THEN 0 ELSE 1 END";
            cmd.Parameters.AddWithValue("@u", username);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;
            return new CentralUser
            {
                Id = r.GetInt32(0), Username = r.GetString(1), Email = r.IsDBNull(2) ? null : r.GetString(2), FullName = r.IsDBNull(3) ? null : r.GetString(3),
                PmsUserId = r.IsDBNull(4) ? null : r.GetInt32(4), LimsUserId = r.IsDBNull(5) ? null : r.GetInt32(5),
                PasswordHash = r.IsDBNull(6) ? null : (byte[])r[6], PasswordKey = r.IsDBNull(7) ? null : (byte[])r[7], IsActive = r.GetBoolean(8)
            };
        }

        public static bool VerifyHmac(string password, byte[]? hash, byte[]? key)
        {
            if (hash == null || key == null || hash.Length == 0 || key.Length == 0) return false;
            using var hmac = new System.Security.Cryptography.HMACSHA512(key);
            var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(computed, hash);
        }

        /// <summary>Store (or refresh) the central credential for a user from a known-good password or an existing PMS hash/key pair.</summary>
        public void StoreCredential(int userId, byte[] hash, byte[] key)
        {
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE dbo.Users SET PasswordHash = @h, PasswordKey = @k, PasswordUpdatedAt = SYSUTCDATETIME() WHERE Id = @id";
            cmd.Parameters.AddWithValue("@h", hash); cmd.Parameters.AddWithValue("@k", key); cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public void StoreCredential(int userId, string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            StoreCredential(userId, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)), hmac.Key);
        }

        /// <summary>Ensure a central user exists for an application-native account (e.g. a LIMS user) and return its id.</summary>
        public int EnsureCentralUser(string username, string? email, string? fullName, int? limsUserId, string defaultRole)
        {
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
DECLARE @uid int = (SELECT TOP 1 Id FROM dbo.Users WHERE Username = @u OR (@e IS NOT NULL AND Email = @e));
IF @uid IS NULL BEGIN
    INSERT INTO dbo.Users (Username, Email, FullName, IsActive, LimsUserId) VALUES (@u, @e, @n, 1, @l);
    SET @uid = SCOPE_IDENTITY();
    INSERT INTO dbo.UserRoles (UserId, RoleId) SELECT @uid, Id FROM dbo.Roles WHERE Code = @r;
END
ELSE UPDATE dbo.Users SET LimsUserId = COALESCE(@l, LimsUserId), Email = COALESCE(Email, @e), FullName = COALESCE(FullName, @n) WHERE Id = @uid;
SELECT @uid;";
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@e", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@n", (object?)fullName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@l", (object?)limsUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@r", defaultRole);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>Open a central session for a central user id (used for non-PMS accounts).</summary>
        public string CreateSessionForUser(int erpUserId, string? ip, string? userAgent, string sourceApp = "PMS")
        {
            var token = NewToken();
            using var con = new SqlConnection(_cs);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"UPDATE dbo.Users SET LastLoginAt = SYSUTCDATETIME() WHERE Id = @uid;
INSERT INTO dbo.Sessions (Token, UserId, ExpiresAt, LastSeenAt, IpAddress, UserAgent, SourceApp) VALUES (@t, @uid, DATEADD(HOUR, @h, SYSUTCDATETIME()), SYSUTCDATETIME(), @ip, @ua, @src);
INSERT INTO dbo.AuditLogs (UserId, App, Event, Detail, IpAddress) VALUES (@uid, @src, 'LOGIN', 'central session opened (central credentials)', @ip);";
            cmd.Parameters.AddWithValue("@uid", erpUserId); cmd.Parameters.AddWithValue("@t", token); cmd.Parameters.AddWithValue("@h", SessionHours);
            cmd.Parameters.AddWithValue("@ip", (object?)Trunc(ip, 64) ?? DBNull.Value); cmd.Parameters.AddWithValue("@ua", (object?)Trunc(userAgent, 300) ?? DBNull.Value); cmd.Parameters.AddWithValue("@src", sourceApp);
            cmd.ExecuteNonQuery();
            return token;
        }

        private static readonly HttpClient _http = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };

        /// <summary>Ask LIMS (over the shared secret) whether these are valid LIMS-native credentials.</summary>
        public (bool ok, int? limsUserId, string? email, string? name, bool isAdmin) VerifyWithLims(string username, string password)
        {
            var upstream = _cfg["Erp:LimsUpstream"]; var secret = _cfg["Erp:SharedSecret"];
            if (string.IsNullOrWhiteSpace(upstream) || string.IsNullOrWhiteSpace(secret)) return (false, null, null, null, false);
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Post, upstream.TrimEnd('/') + "/erp/verify");
                req.Headers.TryAddWithoutValidation("X-Erp-Secret", secret);
                req.Headers.TryAddWithoutValidation("Accept", "application/json");
                req.Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["username"] = username, ["password"] = password });
                using var resp = _http.Send(req);
                if (!resp.IsSuccessStatusCode) return (false, null, null, null, false);
                using var doc = System.Text.Json.JsonDocument.Parse(resp.Content.ReadAsStream());
                var root = doc.RootElement;
                if (!root.TryGetProperty("ok", out var okEl) || !okEl.GetBoolean()) return (false, null, null, null, false);
                var u = root.GetProperty("user");
                return (true, u.GetProperty("id").GetInt32(), u.GetProperty("email").GetString(), u.GetProperty("name").GetString(), u.TryGetProperty("is_admin", out var a) && a.GetInt32() == 1);
            }
            catch (Exception ex) { _log.LogWarning(ex, "LIMS credential verification failed"); return (false, null, null, null, false); }
        }

        public static bool IsToken(string? t) => !string.IsNullOrEmpty(t) && t!.Length == 64 && t.All(Uri.IsHexDigit);

        private static string NewToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        private static string? Trunc(string? s, int n) => s == null ? null : (s.Length <= n ? s : s[..n]);
    }
}
