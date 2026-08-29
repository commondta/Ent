using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WeekSchedule()
        {
            return View();
        }
        public IActionResult WeekScheduleExective()
        {
            return View();
        }
    }
}
