using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    /// <summary>User accounts, roles and role permissions (admin database).</summary>
    public class Security
    {
        readonly Database db;
        public Security(string connectionString) { db = new Database(connectionString); }

        // ---------- authentication ----------

        /// <summary>Returns the session user on success, or null. <paramref name="reason"/> describes the failure.</summary>
        public SessionUser Authenticate(string username, string password, out string reason)
        {
            reason = "Invalid username or password.";
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password)) return null;

            DataTable dt = db.Get("SELECT id, Password, IsActive FROM Account WHERE Username = @u", new SqlParameter("@u", username.Trim()));
            if (dt.Rows.Count != 1) return null;
            DataRow row = dt.Rows[0];
            if (!PasswordHasher.Verify(password, row["Password"].ToString())) return null;
            if (!Convert.ToBoolean(row["IsActive"])) { reason = "This account is disabled or awaiting approval."; return null; }

            SessionUser user = BuildSessionUser(Convert.ToInt32(row["id"]));
            if (user == null) return null;
            reason = null;
            return user;
        }

        /// <summary>Builds the session user (identity + resolved permissions) for an active account and stamps LastLoginAt.</summary>
        public SessionUser BuildSessionUser(int userId)
        {
            DataTable dt = db.Get(@"SELECT a.id, a.Username, a.FullName, a.Email, a.RoleId, r.Name AS RoleName, r.IsSystem, a.IsActive, a.MustChangePassword
                                    FROM Account a LEFT JOIN Role r ON r.id = a.RoleId WHERE a.id = @id", new SqlParameter("@id", userId));
            if (dt.Rows.Count != 1 || !Convert.ToBoolean(dt.Rows[0]["IsActive"])) return null;
            DataRow row = dt.Rows[0];
            SessionUser user = new SessionUser
            {
                id = Convert.ToInt32(row["id"]),
                Username = row["Username"].ToString(),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                RoleId = row["RoleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["RoleId"]),
                RoleName = row["RoleName"].ToString(),
                IsAdministrator = row["IsSystem"] != DBNull.Value && Convert.ToBoolean(row["IsSystem"]) && row["RoleName"].ToString() == "Administrator",
                MustChangePassword = Convert.ToBoolean(row["MustChangePassword"]),
                LoginAt = DateTime.Now
            };
            if (user.RoleId.HasValue)
                foreach (PermissionModel p in GetPermissions(user.RoleId.Value))
                    user.Permissions[p.FormKey] = p;
            db.Set("UPDATE Account SET LastLoginAt = GETDATE() WHERE id = @id", new SqlParameter("@id", user.id));
            return user;
        }

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            DataTable dt = db.Get("SELECT Password FROM Account WHERE id = @id", new SqlParameter("@id", userId));
            if (dt.Rows.Count != 1 || !PasswordHasher.Verify(currentPassword ?? "", dt.Rows[0][0].ToString())) return false;
            db.Set("UPDATE Account SET Password = @p, MustChangePassword = 0 WHERE id = @id",
                new SqlParameter("@p", PasswordHasher.Hash(newPassword)), new SqlParameter("@id", userId));
            return true;
        }

        public void ResetPassword(int userId, string newPassword, bool mustChange)
        {
            db.Set("UPDATE Account SET Password = @p, MustChangePassword = @m WHERE id = @id",
                new SqlParameter("@p", PasswordHasher.Hash(newPassword)), new SqlParameter("@m", mustChange), new SqlParameter("@id", userId));
        }

        // ---------- users ----------

        const string UserSelect = @"SELECT a.id, a.Username, a.FullName, a.Email, a.RoleId, r.Name AS RoleName, a.IsActive, a.MustChangePassword,
                                           a.LastLoginAt, a.CreatedAt, a.CreatedBy
                                    FROM Account a LEFT JOIN Role r ON r.id = a.RoleId ";

        public List<UserModel> GetUsers()
        {
            return ToUsers(db.Get(UserSelect + "ORDER BY a.Username"));
        }

        public UserModel GetUser(int id)
        {
            List<UserModel> l = ToUsers(db.Get(UserSelect + "WHERE a.id = @id", new SqlParameter("@id", id)));
            return l.Count == 0 ? null : l[0];
        }

        public bool UsernameExists(string username, int exceptId)
        {
            DataTable dt = db.Get("SELECT COUNT(*) FROM Account WHERE Username = @u AND id <> @id",
                new SqlParameter("@u", username), new SqlParameter("@id", exceptId));
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public int CreateUser(UserModel u, string createdBy)
        {
            DataTable dt = db.Get(@"INSERT INTO Account(Username, Password, FullName, Email, RoleId, IsActive, MustChangePassword, CreatedBy)
                                    VALUES(@u, @p, @f, @e, @r, @a, @m, @c); SELECT SCOPE_IDENTITY()",
                new SqlParameter("@u", u.Username.Trim()),
                new SqlParameter("@p", PasswordHasher.Hash(u.Password)),
                new SqlParameter("@f", (object)u.FullName ?? DBNull.Value),
                new SqlParameter("@e", (object)u.Email ?? DBNull.Value),
                new SqlParameter("@r", (object)u.RoleId ?? DBNull.Value),
                new SqlParameter("@a", u.IsActive),
                new SqlParameter("@m", u.MustChangePassword),
                new SqlParameter("@c", (object)createdBy ?? DBNull.Value));
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public void UpdateUser(UserModel u)
        {
            db.Set("UPDATE Account SET Username=@u, FullName=@f, Email=@e, RoleId=@r, IsActive=@a WHERE id=@id",
                new SqlParameter("@u", u.Username.Trim()),
                new SqlParameter("@f", (object)u.FullName ?? DBNull.Value),
                new SqlParameter("@e", (object)u.Email ?? DBNull.Value),
                new SqlParameter("@r", (object)u.RoleId ?? DBNull.Value),
                new SqlParameter("@a", u.IsActive),
                new SqlParameter("@id", u.id));
        }

        public void SetUserActive(int id, bool active)
        {
            db.Set("UPDATE Account SET IsActive=@a WHERE id=@id", new SqlParameter("@a", active), new SqlParameter("@id", id));
        }

        public void DeleteUser(int id)
        {
            db.Set("DELETE FROM Account WHERE id=@id", new SqlParameter("@id", id));
        }

        public int CountActiveAdministrators()
        {
            DataTable dt = db.Get("SELECT COUNT(*) FROM Account a JOIN Role r ON r.id=a.RoleId WHERE r.Name='Administrator' AND a.IsActive=1");
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        List<UserModel> ToUsers(DataTable dt)
        {
            List<UserModel> list = new List<UserModel>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new UserModel
                {
                    id = Convert.ToInt32(r["id"]),
                    Username = r["Username"].ToString(),
                    FullName = r["FullName"].ToString(),
                    Email = r["Email"].ToString(),
                    RoleId = r["RoleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["RoleId"]),
                    RoleName = r["RoleName"].ToString(),
                    IsActive = Convert.ToBoolean(r["IsActive"]),
                    MustChangePassword = Convert.ToBoolean(r["MustChangePassword"]),
                    LastLoginAt = r["LastLoginAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["LastLoginAt"]),
                    CreatedAt = Convert.ToDateTime(r["CreatedAt"]),
                    CreatedBy = r["CreatedBy"].ToString()
                });
            }
            return list;
        }

        // ---------- roles ----------

        public List<RoleModel> GetRoles()
        {
            DataTable dt = db.Get(@"SELECT r.id, r.Name, r.Description, r.IsSystem, (SELECT COUNT(*) FROM Account a WHERE a.RoleId=r.id) AS UserCount
                                    FROM Role r ORDER BY r.IsSystem DESC, r.Name");
            List<RoleModel> list = new List<RoleModel>();
            foreach (DataRow r in dt.Rows)
                list.Add(new RoleModel
                {
                    id = Convert.ToInt32(r["id"]),
                    Name = r["Name"].ToString(),
                    Description = r["Description"].ToString(),
                    IsSystem = Convert.ToBoolean(r["IsSystem"]),
                    UserCount = Convert.ToInt32(r["UserCount"])
                });
            return list;
        }

        public RoleModel GetRole(int id)
        {
            foreach (RoleModel r in GetRoles()) if (r.id == id) return r;
            return null;
        }

        public bool RoleNameExists(string name, int exceptId)
        {
            return Convert.ToInt32(db.Get("SELECT COUNT(*) FROM Role WHERE Name=@n AND id<>@id",
                new SqlParameter("@n", name), new SqlParameter("@id", exceptId)).Rows[0][0]) > 0;
        }

        public int SaveRole(RoleModel role)
        {
            if (role.id > 0)
            {
                db.Set("UPDATE Role SET Name=@n, Description=@d WHERE id=@id AND IsSystem=0",
                    new SqlParameter("@n", role.Name.Trim()), new SqlParameter("@d", (object)role.Description ?? DBNull.Value), new SqlParameter("@id", role.id));
                return role.id;
            }
            DataTable dt = db.Get("INSERT INTO Role(Name, Description, IsSystem) VALUES(@n, @d, 0); SELECT SCOPE_IDENTITY()",
                new SqlParameter("@n", role.Name.Trim()), new SqlParameter("@d", (object)role.Description ?? DBNull.Value));
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        /// <summary>Deletes a non-system role that has no users. Returns false otherwise.</summary>
        public bool DeleteRole(int id)
        {
            DataTable dt = db.Get("SELECT IsSystem, (SELECT COUNT(*) FROM Account WHERE RoleId=@id) FROM Role WHERE id=@id", new SqlParameter("@id", id));
            if (dt.Rows.Count == 0 || Convert.ToBoolean(dt.Rows[0][0]) || Convert.ToInt32(dt.Rows[0][1]) > 0) return false;
            db.Set("DELETE FROM Role WHERE id=@id", new SqlParameter("@id", id));
            return true;
        }

        // ---------- permissions ----------

        public List<PermissionModel> GetPermissions(int roleId)
        {
            DataTable dt = db.Get("SELECT RoleId, FormKey, CanView, CanCreate, CanEdit, CanDelete, CanApprove, CanExport, CanPrint FROM RolePermission WHERE RoleId=@r",
                new SqlParameter("@r", roleId));
            List<PermissionModel> list = new List<PermissionModel>();
            foreach (DataRow r in dt.Rows)
                list.Add(new PermissionModel
                {
                    RoleId = roleId,
                    FormKey = r["FormKey"].ToString(),
                    CanView = Convert.ToBoolean(r["CanView"]),
                    CanCreate = Convert.ToBoolean(r["CanCreate"]),
                    CanEdit = Convert.ToBoolean(r["CanEdit"]),
                    CanDelete = Convert.ToBoolean(r["CanDelete"]),
                    CanApprove = Convert.ToBoolean(r["CanApprove"]),
                    CanExport = Convert.ToBoolean(r["CanExport"]),
                    CanPrint = Convert.ToBoolean(r["CanPrint"])
                });
            return list;
        }

        /// <summary>Replaces the whole permission matrix of a role.</summary>
        public void SavePermissions(int roleId, IEnumerable<PermissionModel> permissions)
        {
            db.Set("DELETE FROM RolePermission WHERE RoleId=@r", new SqlParameter("@r", roleId));
            foreach (PermissionModel p in permissions)
            {
                if (string.IsNullOrEmpty(p.FormKey)) continue;
                if (!(p.CanView || p.CanCreate || p.CanEdit || p.CanDelete || p.CanApprove || p.CanExport || p.CanPrint)) continue;
                db.Set(@"INSERT INTO RolePermission(RoleId, FormKey, CanView, CanCreate, CanEdit, CanDelete, CanApprove, CanExport, CanPrint)
                         VALUES(@r, @f, 1, @c, @e, @d, @a, @x, @p)",
                    new SqlParameter("@r", roleId), new SqlParameter("@f", p.FormKey),
                    new SqlParameter("@c", p.CanCreate), new SqlParameter("@e", p.CanEdit),
                    new SqlParameter("@d", p.CanDelete), new SqlParameter("@a", p.CanApprove),
                    new SqlParameter("@x", p.CanExport), new SqlParameter("@p", p.CanPrint));
            }
        }
    }
}
