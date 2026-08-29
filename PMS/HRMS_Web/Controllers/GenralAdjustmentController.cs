using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class GenralAdjustmentController : Controller
    {
        public IActionResult GenralAdjustment()
        {
            return View();
        }

        public IActionResult StandAlone()
        {
            return View();
        }
    }
}
