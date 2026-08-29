using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult FormAlerts()
        {
            return View();
        }
        public IActionResult SoftLockName()
        {
            return View();
        }
        public IActionResult AlertName()
        {
            return View();
        }
    }
}
