using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class Operations : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FileRequest()
        {
            return View();
        }
        public IActionResult FileVerificationRequest(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult FileVerificationNDC1()
        {
            return View();
        }
        public IActionResult ClientFileReceiving(int? id )
        {
            ViewBag.Id =id;
            return View();
        }
        public IActionResult COP()
        {
            return View();
        }

        public IActionResult DeAllocation()
        {
            return View();
        }
        public IActionResult ReNumber()
        {
            return View();
        }
        public IActionResult RePurchase()
        {
            return View();
        }
        public IActionResult MemberNDC(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult DealerNDC()
        {
            return View();
        }
        public IActionResult NDC1()
        {
            return View();
        }

        public IActionResult TransferSetReceiving()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            return View();
        }

        public IActionResult TransferType()
        {
            return View();
        }

        public IActionResult TaxType()
        {
            return View();
        }

        public IActionResult NDCRequestType()
        {
            return View();
        }

        public IActionResult Surrender()
        {
            return View();
        }
        public IActionResult ReSurrender()
        {
            return View();
        }

        public IActionResult TransferTaxEstimation()
        {
            return View();
        }

        public IActionResult Propertybinding()
        {
            return View();
        }

        public IActionResult Amalgamation()
        {
            return View();
        }

        // HIDDEN-FORM REVIEW (#133) — Views/Operations/TransferForm.cshtml (386 lines) had no action
        // at all, so the view was dead. Added for your review pass. To reverse: delete this method.
        public IActionResult TransferForm()
        {
            return View();
        }
    }
}
