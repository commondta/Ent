using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Infrastructure
{
    /// <summary>One application of the ERP suite (mirrors HRMS_Web.Services.ErpPlatform.ErpApplication).</summary>
    [Serializable]
    public class ErpApplication
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaseUrl { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// ERP platform single sign-on for Payroll (the Enterprise Solution pattern, same as LIMS' ErpSso):
    /// the central login (PMS) sets an <c>erp_sso</c> cookie whose token is a row in ERP_Platform.dbo.Sessions;
    /// this host is reverse-proxied under /payroll on the same origin so the cookie arrives here.
    /// We validate the token, check the user is entitled to the PAYROLL application, map (or provision) the
    /// local account, and build the normal <see cref="SessionUser"/>. Form-level permissions stay local (roles).
    /// </summary>
    public static class ErpSso
    {
        public static bool Enabled
        {
            get
            {
                return string.Equals(ConfigurationManager.AppSettings["Erp:Enabled"], "true", StringComparison.OrdinalIgnoreCase)
                    && ConfigurationManager.ConnectionStrings["ErpPlatform"] != null;
            }
        }
        public static string BaseUrl { get { return (ConfigurationManager.AppSettings["Erp:BaseUrl"] ?? "").TrimEnd('/'); } }
        public static string AppCode { get { return ConfigurationManager.AppSettings["Erp:AppCode"] ?? "PAYROLL"; } }
        public static string CookieName { get { return ConfigurationManager.AppSettings["Erp:CookieName"] ?? "erp_sso"; } }
        public static string SharedSecret { get { return ConfigurationManager.AppSettings["Erp:SharedSecret"] ?? ""; } }
        public static string LoginUrl { get { return BaseUrl + "/Login/Index"; } }
        public static string LogoutUrl { get { return BaseUrl + "/Login/SignOut"; } }
        public static string AppsUrl { get { return BaseUrl + "/Apps"; } }
        public static string GoUrl(string code) { return BaseUrl + "/Apps/Go?code=" + HttpUtility.UrlEncode(code); }

        public const string AppsSessionKey = "erp_apps";
        public const string TokenSessionKey = "erp_sso";

        static Database Db { get { return new Database(ConfigurationManager.ConnectionStrings["ErpPlatform"].ConnectionString); } }

        public static bool IsToken(string t) { return !string.IsNullOrEmpty(t) && t.Length == 64 && t.All(Uri.IsHexDigit); }

        /// <summary>Token from the request cookie, or null.</summary>
        public static string CookieToken(HttpRequestBase req)
        {
            HttpCookie c = req.Cookies[CookieName];
            return c != null && IsToken(c.Value) ? c.Value : null;
        }

        public class CentralIdentity
        {
            public int UserId; public string Username; public string FullName; public string Email;
            public List<string> Roles = new List<string>();
            public List<ErpApplication> Apps = new List<ErpApplication>();
            public bool IsErpAdmin { get { return Roles.Contains("ERP_ADMIN"); } }
            public bool HasApp(string code) { return Apps.Any(a => a.IsActive && string.Equals(a.Code, code, StringComparison.OrdinalIgnoreCase)); }
        }

        /// <summary>Validates a session token against the central database. Null when invalid/expired/revoked.</summary>
        public static CentralIdentity Validate(string token)
        {
            if (!IsToken(token)) return null;
            Database db = Db;
            DataTable u = db.Get(@"SELECT u.Id, u.Username, u.FullName, u.Email
                                   FROM dbo.Sessions s JOIN dbo.Users u ON u.Id = s.UserId
                                   WHERE s.Token = @t AND s.RevokedAt IS NULL AND s.ExpiresAt > SYSUTCDATETIME() AND u.IsActive = 1",
                new SqlParameter("@t", token));
            if (u.Rows.Count != 1) return null;
            CentralIdentity id = new CentralIdentity
            {
                UserId = Convert.ToInt32(u.Rows[0]["Id"]),
                Username = u.Rows[0]["Username"].ToString(),
                FullName = u.Rows[0]["FullName"].ToString(),
                Email = u.Rows[0]["Email"].ToString()
            };
            foreach (DataRow r in db.Get("SELECT r.Code FROM dbo.UserRoles ur JOIN dbo.Roles r ON r.Id = ur.RoleId WHERE ur.UserId = @u", new SqlParameter("@u", id.UserId)).Rows)
                id.Roles.Add(r[0].ToString());
            foreach (DataRow r in db.Get(@"SELECT DISTINCT a.Code, a.Name, a.Description, a.BaseUrl, a.SortOrder, a.IsActive
                                           FROM dbo.UserRoles ur JOIN dbo.RoleApplication ra ON ra.RoleId = ur.RoleId
                                           JOIN dbo.Applications a ON a.Id = ra.ApplicationId WHERE ur.UserId = @u ORDER BY a.SortOrder",
                                           new SqlParameter("@u", id.UserId)).Rows)
                id.Apps.Add(new ErpApplication
                {
                    Code = r["Code"].ToString(), Name = r["Name"].ToString(), Description = r["Description"].ToString(),
                    BaseUrl = r["BaseUrl"].ToString(), SortOrder = Convert.ToInt32(r["SortOrder"]), IsActive = Convert.ToBoolean(r["IsActive"])
                });
            db.Set("UPDATE dbo.Sessions SET LastSeenAt = SYSUTCDATETIME() WHERE Token = @t", new SqlParameter("@t", token));
            return id;
        }

        /// <summary>Revokes the central session (global sign-out).</summary>
        public static void Revoke(string token)
        {
            if (!IsToken(token)) return;
            try
            {
                Db.Set(@"UPDATE dbo.Sessions SET RevokedAt = SYSUTCDATETIME() WHERE Token = @t AND RevokedAt IS NULL;
                         INSERT INTO dbo.AuditLogs (UserId, App, Event, Detail) SELECT UserId, @app, 'LOGOUT', 'central session revoked' FROM dbo.Sessions WHERE Token = @t;",
                    new SqlParameter("@t", token), new SqlParameter("@app", AppCode));
            }
            catch (Exception ex) { FileLogger.Error("ERP session revoke failed.", ex); }
        }

        /// <summary>
        /// Signs the local session in from the central identity: maps to the local Account by username
        /// (provisioned on first visit - ERP administrators become Administrator, everyone else Viewer,
        /// which a Payroll administrator can then raise under User Management). Returns null when the
        /// user is not entitled to this application.
        /// </summary>
        public static SessionUser SignIn(HttpContextBase ctx, string token, CentralIdentity id)
        {
            if (id == null || !id.HasApp(AppCode)) return null;

            Security sec = App.Security;
            UserModel local = sec.GetUsers().FirstOrDefault(x => string.Equals(x.Username, id.Username, StringComparison.OrdinalIgnoreCase));
            if (local == null)
            {
                RoleModel role = sec.GetRoles().FirstOrDefault(r => r.Name == (id.IsErpAdmin ? "Administrator" : "Viewer"));
                int newId = sec.CreateUser(new UserModel
                {
                    Username = id.Username, FullName = id.FullName, Email = id.Email, RoleId = role == null ? (int?)null : role.id,
                    IsActive = true, MustChangePassword = false,
                    Password = Guid.NewGuid().ToString("N") + "Aa1" // unusable locally; sign-in is central
                }, "erp-sso");
                local = sec.GetUser(newId);
                try { App.Activity.Log(id.Username, "Security", "Security", "Security.Users", "Account provisioned from ERP single sign-on (" + (local.RoleName ?? "no role") + ")", App.ClientIp); } catch { }
            }
            else if (!local.IsActive) return null;

            SessionUser user = sec.BuildSessionUser(local.id);
            if (user == null) return null;

            object activeComp = ctx.Session["activeComp"];
            ctx.Session.Clear();
            ctx.Session[App.SessionUserKey] = user;
            ctx.Session["activeComp"] = activeComp;
            ctx.Session[TokenSessionKey] = token;
            ctx.Session[AppsSessionKey] = id.Apps;
            ctx.Session["erp_checked"] = DateTime.UtcNow;
            try { App.Activity.Log(user.Username, "Login", "Security", null, "Signed in via ERP single sign-on as " + user.RoleName, App.ClientIp); } catch { }
            return user;
        }

        /// <summary>Applications the current user may switch to (from session), or an empty list.</summary>
        public static List<ErpApplication> Apps(HttpSessionStateBase session)
        {
            return (session == null ? null : session[AppsSessionKey] as List<ErpApplication>) ?? new List<ErpApplication>();
        }
    }
}
