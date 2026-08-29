using BusinessLayer;
using Payroll_HCC.Infrastructure;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Payroll_HCC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;

            // Security schema (roles, permissions, activity log, approvals) - idempotent, safe to run every start.
            try
            {
                SchemaUpgrade.Apply(ConfigurationManager.ConnectionStrings["Payroll_HCC"].ConnectionString);
            }
            catch (Exception ex)
            {
                FileLogger.Error("Security schema upgrade failed - sign-in will not work until the admin database is reachable.", ex);
            }
            FileLogger.Info("Application started.");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Show the active company name in the layout. A DB hiccup here must not
            // take down every new session - degrade gracefully and log instead.
            try
            {
                Company compObj = new Company(ConfigurationManager.ConnectionStrings["Payroll_HCC"].ConnectionString);
                Session["activeComp"] = compObj.getActiveName();
            }
            catch (Exception ex)
            {
                Session["activeComp"] = "";
                FileLogger.Error("Session_Start: failed to load active company name.", ex);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex == null) return;

            var ctx = HttpContext.Current;
            string url = ctx != null && ctx.Request != null ? ctx.Request.RawUrl : "(no request)";
            FileLogger.Error("Unhandled exception at " + url, ex);
        }
    }
}
