using HRMS_Web.Models.DTOs.SMSDTO;
using HRMS_Web.Services.SMSService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly ISMSService _smsService;

        public SMSController(ISMSService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("SendSingleSms")]
        public async Task<IActionResult> SendSingleSms([FromBody] SingleSMSRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message) || string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                return BadRequest("Message and mobile number are required.");
            }

            var result = await _smsService.SendSingleSmsAsync(request.MobileNumber,request.Message);
            return Ok(result);
        }

        [HttpPost("SendMultiSms")]
        public async Task<IActionResult> SendMultiSms([FromBody] MultiSMSRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message) || request.MobileNumbers == null || request.MobileNumbers.Length == 0)
            {
                return BadRequest("Message and at least one mobile number are required.");
            }

            var result = await _smsService.SendMultiSmsAsync(request.MobileNumbers, request.Message);
            return Ok(result);
        }
    }
}
