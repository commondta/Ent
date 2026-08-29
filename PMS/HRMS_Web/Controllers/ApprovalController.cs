using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class ApprovalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ApprovalSetup()
        {
            return View();
        }
        public IActionResult ViewApproval()
        {
            return View();
        }
        public IActionResult Inbox()
        {
            return View();
        }
        public IActionResult Permission()
        {
            return View();
        }
    }
}
