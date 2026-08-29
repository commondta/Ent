using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{



    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyBindingControll : Controller
    {




        [HttpGet]
        [Route("GetAllPropertybindingList")]
        public IActionResult GetAllPropertybindingList()
        {
            return View();
        }
    }
}
