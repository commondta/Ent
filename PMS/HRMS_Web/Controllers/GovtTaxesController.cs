using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class GovtTaxesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TransferReceiptProcessing(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult BookingReceiptProcessing()
        {
            return View();
        }
    }
}
