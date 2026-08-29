using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class GlobalSetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChargesSetup()
        {
            return View();
        }
        public IActionResult ChargesGroup()
        {
            return View();
        }
        public IActionResult ChargesType()
        {
            return View();
        }
        public IActionResult ChargesGroupForm()
        {
            return View();
        }
        public IActionResult ChargesGroupFormTest()
        {
            return View();
        } public IActionResult ViolationGroup()
        {
            return View();
        }
        public IActionResult ViolationType()
        {
            return View();
        }
    }
}
