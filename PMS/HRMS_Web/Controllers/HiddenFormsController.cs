using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    // HIDDEN-FORM REVIEW (#133) — temporary scaffolding, not part of the product.
    // Serves one page at /HiddenForms that links every form the menu cannot reach,
    // so they can be opened and judged (restore / retire) in one sitting.
    // To reverse: delete this file and Views/HiddenForms/.
    public class HiddenFormsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
