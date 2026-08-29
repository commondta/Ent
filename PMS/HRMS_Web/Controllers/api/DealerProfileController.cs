using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.BusinessServicesInterFace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DealerProfileController : ControllerBase
    {
        private readonly IDealerProfile _iDealerProfile;

        public DealerProfileController(IDealerProfile iDealerProfile)
        {
            _iDealerProfile = iDealerProfile;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id) => Ok(_iDealerProfile.Get(id));

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllReferralCode() => Ok(_iDealerProfile.GetAll());

        [HttpPost]
        [Route("AddDealerProfile")]
        public IActionResult AddDealerProfile([FromBody] DealerProfile dto)
        {
            try
            {
                dto.Id = _iDealerProfile.Create(dto);

                if (dto.Id == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "ReferralCode creation failed! Please check ReferralCode details and try again." });
                }
                else if (dto.Id > 0)
                {
                    return Ok(new Response { Status = "Success", Message = "Dealer Profile created successfully!" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new Response { Status = "Error", Message = "Not Excepted Values" });
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Status = "Exception", Message = "There Is Problem With Response" + message });
            }
        }

        [HttpPut]
        [Route("UpdateDealerProfile")]
        public IActionResult UpdateDealerProfile([FromBody] DealerProfile dto)
        {
            try
            {
                if (dto == null || dto.Id == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Invalid ReferralCode." });
                }

                _iDealerProfile.Update(dto);

                return Ok(new Response { Status = "Success", Message = "Dealer Profile Updated successfully!" });
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Status = "Exception", Message = "There Is Problem With Response" + message });
            }
        }

        [HttpDelete]
        [Route("DeleteDealerProfile")]
        public IActionResult DeleteDealerProfile(int id)
        {
            if (id > 0)
            {
                _iDealerProfile.Delete(id);

                return Ok(new Response { Status = "Success", Message = "Dealer Profile has been Deleted!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Not Found", Message = "Record Not Found.!" });
            }
        }
    }
}
