using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class PromotionController : Controller
    {
        public IActionResult Promotions()
        {
            return View();
        }
        public IActionResult Banners()
        {
            return View();
        }
    }
}
