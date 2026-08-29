using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class ReceiptController : Controller
    {
        public IActionResult Receipt()
        {
            return View();
        }
    }
}
