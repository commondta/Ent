using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class PermissionSetupController : Controller
    {
        public IActionResult PermissionForm()
        {
            return View();
        }
        
        public IActionResult ApprovalUISetup()
        {
            return View();
        }
    }
}
