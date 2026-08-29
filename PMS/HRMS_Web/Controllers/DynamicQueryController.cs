using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class DynamicQueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DynamicReport()
        {
            return View();
        }
    }
}
