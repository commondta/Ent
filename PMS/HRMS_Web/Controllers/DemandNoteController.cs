using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class DemandNoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DemandNoteForm()
        {
            return View();
        }
        public IActionResult DNHOD()
        {
            return View();
        }
        public IActionResult DNCustodian()
        {
            return View();
        }

        // HIDDEN-FORM REVIEW (#133) — action was commented out, so Views/DemandNote/PurchaseRequest.cshtml
        // (719 lines) could never be reached. Restored for your review pass. To reverse: comment out again.
        public IActionResult PurchaseRequest()
        {
            return View();
        }
    }
}
