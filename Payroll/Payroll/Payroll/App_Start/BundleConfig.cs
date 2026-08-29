using System.Web;
using System.Web.Optimization;

namespace Payroll_HCC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundle/js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundle/css").Include(
                      "~/Content/css/admin.css",
                      "~/Content/css/AdminLTE.min.css",
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/bootstrap-material-design.min.css",
                      "~/Content/css/custom-theme.css",
                      "~/Content/css/datatables.min.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/master.css",
                      "~/Content/css/MaterialAdminLTE.min.css",
                      "~/Content/css/morris.css",
                      "~/Content/css/ripples.min.css",
                      "~/Content/css/skin-md-blue.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
