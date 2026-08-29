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
    public class BFeatureController : ControllerBase
    {
        private readonly IFeatures _iFeatures;

        public BFeatureController(IFeatures iFeatures)
        {
            _iFeatures = iFeatures;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id) => Ok(_iFeatures.Get(id));

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllReferralCode() => Ok(_iFeatures.GetAll());

        [HttpPost]
        [Route("AddFeature")]
        public IActionResult AddFeature([FromBody] FeatureDTO dto)
        {
            try
            {
                dto.ID = _iFeatures.Create(dto);

                if (dto.ID == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "ReferralCode creation failed! Please check ReferralCode details and try again." });
                }
                else if (dto.ID > 0)
                {
                    return Ok(new Response { Status = "Success", Message = "Feature created successfully!" });
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
        [Route("UpdateFeature")]
        public IActionResult UpdateFeature([FromBody] FeatureDTO dto)
        {
            try
            {
                if (dto == null || dto.ID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Invalid ReferralCode." });
                }

                _iFeatures.Update(dto);

                return Ok(new Response { Status = "Success", Message = "Feature Updated successfully!" });
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Status = "Exception", Message = "There Is Problem With Response" + message });
            }
        }

        [HttpDelete]
        [Route("DeleteFeature")]
        public IActionResult DeleteFeature(int id)
        {
            if (id > 0)
            {
                _iFeatures.Delete(id);

                return Ok(new Response { Status = "Success", Message = "Feature has been Deleted!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Not Found", Message = "Record Not Found.!" });
            }
        }
    }
}
