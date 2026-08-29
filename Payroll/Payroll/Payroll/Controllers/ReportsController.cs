using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll_HCC.Controllers
{
    [Payroll_HCC.Filters.AdminAuthorize]
    public class ReportsController : Controller
    {
        static string conStringHCC = ConfigurationManager.ConnectionStrings["Payroll_HCC"].ConnectionString;
        Company compObj = new Company(conStringHCC);

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Company = compObj.getAll();
        }
        // GET: Reports
        public ActionResult AddressDetail()
        {
            return View();
        }

        public ActionResult BankDetail()
        {
            return View();
        }

        public ActionResult JobDetail()
        {
            return View();
        }

        public ActionResult PersonalDetail()
        {
            return View();
        }
    }
}