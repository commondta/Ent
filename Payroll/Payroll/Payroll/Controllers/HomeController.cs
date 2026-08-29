using BusinessLayer;
using DataLayer;
using Payroll_HCC.Filters;
using Payroll_HCC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Payroll_HCC.Controllers
{
    /// <summary>Workspace home: module launcher, real KPIs, recent activity, pending approvals.</summary>
    [AdminAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SessionUser user = App.CurrentUser;

            // Modules the user can open (a module is visible when at least one of its forms is).
            ViewBag.Modules = FormRegistry.Modules
                .Where(m => m.Key != "Home" && m.Forms.Any(f => user.CanView(f.Key)))
                .ToList();

            ViewBag.Kpi = LoadKpis(user);
            ViewBag.Recent = App.Activity.Recent(10, App.Can("Security.ActivityLog", PermissionAction.View) ? null : user.Username);
            ViewBag.PendingApprovals = App.Can("Security.Approvals", PermissionAction.View) ? App.Approvals.List(BusinessLayer.Approvals.Pending, 6) : new List<ApprovalRequestModel>();
            ViewBag.MyReturned = App.Approvals.MyRequests(user.Username, 20).Where(r => r.Status == BusinessLayer.Approvals.Returned).ToList();
            return View();
        }

        /// <summary>Real figures from the active company database; null when a figure is unavailable (never invented).</summary>
        Dictionary<string, string> LoadKpis(SessionUser user)
        {
            var kpi = new Dictionary<string, string>();
            try
            {
                Database company = new Database(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (user.CanView("Employees.Profiles"))
                    kpi["Employees"] = company.Get("SELECT COUNT(*) FROM EmployeeDetail").Rows[0][0].ToString();
                if (user.CanView("Employees.Departments"))
                    kpi["Departments"] = company.Get("SELECT COUNT(*) FROM DepartmentSetup").Rows[0][0].ToString();
                if (user.CanView("Setup.PayrollPeriods"))
                {
                    var dt = company.Get("SELECT TOP 1 Name, FromDate, ToDate FROM PayPeriod WHERE @today BETWEEN FromDate AND ToDate ORDER BY FromDate DESC",
                        new SqlParameter("@today", DateTime.Today));
                    if (dt.Rows.Count == 0)
                        dt = company.Get("SELECT TOP 1 Name, FromDate, ToDate FROM PayPeriod ORDER BY ToDate DESC");
                    kpi["Period"] = dt.Rows.Count == 0 ? null : dt.Rows[0]["Name"].ToString();
                }
                if (user.CanView("Payroll.Processing"))
                {
                    var dt = company.Get("SELECT TOP 1 PayMonth, Status FROM PayrollProcessParent ORDER BY DocumentDate DESC, DocumentNo DESC");
                    kpi["LastPayroll"] = dt.Rows.Count == 0 ? null : (dt.Rows[0]["PayMonth"] + " · " + dt.Rows[0]["Status"]);
                }
            }
            catch (Exception ex)
            {
                FileLogger.Error("Home KPIs unavailable.", ex);
            }
            if (user.CanView("Security.Approvals")) kpi["Pending"] = App.Approvals.PendingCount().ToString();
            if (user.CanView("Security.Users")) kpi["Users"] = App.Security.GetUsers().Count(u => u.IsActive).ToString();
            return kpi;
        }

        /// <summary>Global search: forms/modules (always) plus employees when the user may view them. Respects permissions.</summary>
        [HttpGet]
        public ActionResult Search(string q)
        {
            q = (q ?? "").Trim();
            SessionUser user = App.CurrentUser;
            var results = new List<object>();
            if (q.Length == 0) return Json(results, JsonRequestBehavior.AllowGet);

            foreach (FormInfo f in FormRegistry.All)
            {
                if (!user.CanView(f.Key)) continue;
                if (Contains(f.Title, q) || Contains(f.LegacyTitle, q) || Contains(f.Module.Title, q))
                    results.Add(new { type = "Form", title = f.Title, sub = f.Module.Title, url = Url.Content("~" + f.Url), icon = f.Icon });
                if (results.Count >= 8) break;
            }

            if (q.Length >= 2 && user.CanView("Employees.Profiles"))
            {
                try
                {
                    Database company = new Database(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    var dt = company.Get(@"SELECT TOP 6 id, EmployeeNumber, PayrollName, JobTitlePosition, Department FROM EmployeeDetail
                                           WHERE PayrollName LIKE @q OR EmployeeNumber LIKE @q OR LegalFirstName LIKE @q OR LegalLastName LIKE @q ORDER BY PayrollName",
                        new SqlParameter("@q", "%" + q + "%"));
                    foreach (System.Data.DataRow r in dt.Rows)
                        results.Add(new { type = "Employee", title = r["PayrollName"].ToString(), sub = string.Join(" - ", new[] { r["EmployeeNumber"].ToString(), r["JobTitlePosition"].ToString(), r["Department"].ToString() }.Where(x => x.Length > 0)), url = Url.Content("~/Master/Employees?q=") + Server.UrlEncode(r["EmployeeNumber"].ToString()), icon = "person-badge" });
                }
                catch (Exception ex) { FileLogger.Error("Employee search failed.", ex); }
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        static bool Contains(string hay, string needle)
        {
            return hay != null && hay.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}
