using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult TransferSetReceivingReport()
        {
            return View();
        }
        public IActionResult TaxReport()
        {
            return View();
        }
        public IActionResult NdcStateReport()
        {
            return View();
        }
        public IActionResult RecordRoomReport()
        {
            return View();
        }
        public IActionResult FileInOutReport()
        {
            return View();
        }
        public IActionResult CancelReStrotionReport()
        {
            return View();
        }
        public IActionResult AllocationReport()
        {
            return View();
        }

        public IActionResult MemberReport()
        {
            return View();
        }

        public IActionResult DealerReport()
        {
            return View();
        }
        public IActionResult CautionReport()
        {
            return View();
        }
        public IActionResult TransferReport()
        {
            return View();
        }
        public IActionResult TransferRevenueReport()
        {
            return View();
        }
    }
}
