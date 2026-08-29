using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    /// <summary>Audit trail of user activity (admin database). Feeds "Recent activity" in the header and dashboard.</summary>
    public class ActivityLog
    {
        readonly Database db;
        public ActivityLog(string connectionString) { db = new Database(connectionString); }

        public void Log(string username, string action, string module, string formKey, string detail, string ip)
        {
            if (detail != null && detail.Length > 500) detail = detail.Substring(0, 500);
            db.Set(@"INSERT INTO ActivityLog(Username, Action, Module, FormKey, Detail, IpAddress) VALUES(@u, @a, @m, @f, @d, @i)",
                new SqlParameter("@u", username ?? "system"),
                new SqlParameter("@a", action ?? "Info"),
                new SqlParameter("@m", (object)module ?? DBNull.Value),
                new SqlParameter("@f", (object)formKey ?? DBNull.Value),
                new SqlParameter("@d", (object)detail ?? DBNull.Value),
                new SqlParameter("@i", (object)ip ?? DBNull.Value));
        }

        public List<ActivityLogModel> Recent(int count, string username)
        {
            string sql = "SELECT TOP (@n) * FROM ActivityLog " + (username == null ? "" : "WHERE Username=@u ") + "ORDER BY id DESC";
            List<SqlParameter> ps = new List<SqlParameter> { new SqlParameter("@n", count) };
            if (username != null) ps.Add(new SqlParameter("@u", username));
            return ToList(db.Get(sql, ps.ToArray()));
        }

        public List<ActivityLogModel> Query(DateTime? from, DateTime? to, string username, string action, string text, int max)
        {
            string sql = "SELECT TOP (@n) * FROM ActivityLog WHERE 1=1";
            List<SqlParameter> ps = new List<SqlParameter> { new SqlParameter("@n", max) };
            if (from.HasValue) { sql += " AND OccurredAt >= @from"; ps.Add(new SqlParameter("@from", from.Value.Date)); }
            if (to.HasValue) { sql += " AND OccurredAt < @to"; ps.Add(new SqlParameter("@to", to.Value.Date.AddDays(1))); }
            if (!string.IsNullOrWhiteSpace(username)) { sql += " AND Username = @u"; ps.Add(new SqlParameter("@u", username.Trim())); }
            if (!string.IsNullOrWhiteSpace(action)) { sql += " AND Action = @a"; ps.Add(new SqlParameter("@a", action.Trim())); }
            if (!string.IsNullOrWhiteSpace(text)) { sql += " AND (Detail LIKE @t OR Module LIKE @t OR FormKey LIKE @t)"; ps.Add(new SqlParameter("@t", "%" + text.Trim() + "%")); }
            sql += " ORDER BY id DESC";
            return ToList(db.Get(sql, ps.ToArray()));
        }

        public int CountSince(DateTime since)
        {
            return Convert.ToInt32(db.Get("SELECT COUNT(*) FROM ActivityLog WHERE OccurredAt >= @s", new SqlParameter("@s", since)).Rows[0][0]);
        }

        public int CountLoginsSince(DateTime since)
        {
            return Convert.ToInt32(db.Get("SELECT COUNT(DISTINCT Username) FROM ActivityLog WHERE Action='Login' AND OccurredAt >= @s", new SqlParameter("@s", since)).Rows[0][0]);
        }

        List<ActivityLogModel> ToList(DataTable dt)
        {
            List<ActivityLogModel> list = new List<ActivityLogModel>();
            foreach (DataRow r in dt.Rows)
                list.Add(new ActivityLogModel
                {
                    id = Convert.ToInt64(r["id"]),
                    OccurredAt = Convert.ToDateTime(r["OccurredAt"]),
                    Username = r["Username"].ToString(),
                    Action = r["Action"].ToString(),
                    Module = r["Module"].ToString(),
                    FormKey = r["FormKey"].ToString(),
                    Detail = r["Detail"].ToString(),
                    IpAddress = r["IpAddress"].ToString()
                });
            return list;
        }
    }
}
