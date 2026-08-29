using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class BillingController : Controller
    {
        public IActionResult MeterInstallation()
        {
            return View();
        }
        public IActionResult MeterReading()
        {
            return View();
        }
        public IActionResult MeterType()
        {
            return View();
        }
        public IActionResult MeterPhase()
        {
            return View();
        }
        public IActionResult MeterPhaseWiseRate()
        {
            return View();
        }
        public IActionResult IndividualBill()
        {
            return View();
        }
        public IActionResult MeterBillGeneration()
        {
            return View();
        }

        public IActionResult MeterBillGenerationOneGo()
        {
            return View();
        }
        public IActionResult MeterStatus()
        {
            return View();
        }

        public IActionResult ReadingOfficer()
        {
            return View();
        }
        public IActionResult FixedChargeGeneration()
        {
            return View();
        }
        public IActionResult MonthlyBillGeneration()
        {
            return View();
        }

        public IActionResult MonthlyBillGenerationBackLog()
        {
            return View();
        }

        public IActionResult GracePeriodSetup()
        {
            return View();
        }

        public IActionResult WithHoldingTax()
        {
            return View();
        }

        public IActionResult SaleTax()
        {
            return View();
        }

        public IActionResult FixedBillGenerationPropertyWise()
        {
            return View();
        }
    }
}
