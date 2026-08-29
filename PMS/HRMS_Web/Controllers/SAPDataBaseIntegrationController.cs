using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class SAPDataBaseIntegrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SAPOperations()
        {
            return View();
        }
        public IActionResult SAPBilling()
        {
            return View();
        }

        public IActionResult GLDetermination()
        {
            return View();
        }
    }
}
