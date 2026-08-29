using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }
        public IActionResult InventoryDashboard()
        {
            return View();
        }
        public IActionResult AllotedInventoryDashboard()
        {
            return View();
        }

        public IActionResult AvailableInventoryDashboard()
        {
            return View();
        }
        public IActionResult TransferDashboard()
        {
            return View();
        }
        public IActionResult NDCDashboard()
        {
            return View();
        }
        public IActionResult MemberDashboard()
        {
            return View();
        }
        public IActionResult SalesDashboard()
        {
            return View();
        }
    }
}
