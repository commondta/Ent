using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class Sales : Controller
    {
        public IActionResult MemberProfile(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult KYCForm()
        {
            return View();
        }
        public IActionResult BookingForm(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult BookingBacklog()
        {
            return View();
        }
        public IActionResult DealSetup()
        {
            return View();
        }
        public IActionResult DealerProfile(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult DealerReservation()
        {
            return View();
        }
        public IActionResult DealMerger()
        {
            return View();
        }
        public IActionResult AdvanceApp()
        {
            return View();
        }
        public IActionResult RenewalForm()
        {
            return View();
        }
        public IActionResult PaymentPlanType()
        {
            return View();
        }
        public IActionResult PaymentPlanSetup()
        {
            return View();
        }
        public IActionResult DealerRegistration()
        {
            return View();
        }
        public IActionResult MemberRegistration()
        {
            return View();
        }
        public IActionResult LeadGeneration()
        {
            return View();
        }
        public IActionResult PreSaleApproval(int? id)
        {
            ViewBag.Id = id;
            return View();
        }   
        public IActionResult DealerCategory()
        {
            return View();
        } 
        public IActionResult DealerDesignation()
        {
            return View();
        }

        public IActionResult PaymentPlanBinding()
        {
            return View();
        }

    }
}
