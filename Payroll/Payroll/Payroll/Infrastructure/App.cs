using BusinessLayer;
using DataLayer;
using System;
using System.Configuration;
using System.Web;

namespace Payroll_HCC.Infrastructure
{
    /// <summary>Request-scoped helpers: current user, admin-db services and activity logging.</summary>
    public static class App
    {
        public const string SessionUserKey = "User";
        public const string ProductName = "Payroll Management";
        public const string CompanyName = "N-Stack";

        public static string AdminConnection
        {
            get { return ConfigurationManager.ConnectionStrings["Payroll_HCC"].ConnectionString; }
        }

        public static SessionUser CurrentUser
        {
            get
            {
                HttpContext ctx = HttpContext.Current;
                return ctx == null || ctx.Session == null ? null : ctx.Session[SessionUserKey] as SessionUser;
            }
        }

        public static string CurrentUsername
        {
            get { SessionUser u = CurrentUser; return u == null ? "anonymous" : u.Username; }
        }

        public static Security Security { get { return new Security(AdminConnection); } }
        public static ActivityLog Activity { get { return new ActivityLog(AdminConnection); } }
        public static Approvals Approvals { get { return new Approvals(AdminConnection); } }

        public static bool Can(string formKey, PermissionAction action)
        {
            SessionUser u = CurrentUser;
            return u != null && u.Can(formKey, action);
        }

        public static string ClientIp
        {
            get
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx == null) return null;
                string ip = ctx.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip)) ip = ctx.Request.UserHostAddress;
                return ip;
            }
        }

        /// <summary>Writes an activity entry; never throws (a logging failure must not break the business action).</summary>
        public static void Log(string action, string formKey, string detail)
        {
            try
            {
                FormInfo f = FormRegistry.ByKey(formKey);
                Activity.Log(CurrentUsername, action, f == null ? null : f.Module.Title, formKey, detail, ClientIp);
            }
            catch (Exception ex)
            {
                FileLogger.Error("Activity log write failed.", ex);
            }
        }

        public static string TimeAgo(DateTime when)
        {
            TimeSpan s = DateTime.Now - when;
            if (s.TotalSeconds < 60) return "just now";
            if (s.TotalMinutes < 60) return (int)s.TotalMinutes + " min ago";
            if (s.TotalHours < 24) return (int)s.TotalHours + " h ago";
            if (s.TotalDays < 7) return (int)s.TotalDays + " d ago";
            return when.ToString("dd MMM yyyy");
        }
    }
}
