using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class DealController : Controller
    {
        public IActionResult Deals()
        {
            return View();
        }
        public IActionResult BulkDeal()
        {
            return View();
        }
    }
}
