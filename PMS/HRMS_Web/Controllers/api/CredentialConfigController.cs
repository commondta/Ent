using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialConfigController : ControllerBase
    {
        private readonly DataBase_Context _context;

        public CredentialConfigController(DataBase_Context context)
        {
            _context = context;
        }


        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var config = await _context.CredentialConfig.FirstOrDefaultAsync();

            if (config == null)
            {
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "No record found",
                    Data = null
                });
            }

            var dto = new CredentialConfigDto
            {
                Id = config.Id,
                SmtpHost = config.SmtpHost,
                SmtpPort = config.SmtpPort,
                SmtpUsername = config.SmtpUsername,
                SmtpPassword = config.SmtpEncryptedPassword.Decrypt(),
                SmtpEncryptionType = config.SmtpEncryptionType,
                FromEmail = config.FromEmail,

                TelecardApiUsername = config.TelecardApiUsername,
                TelecardPassword = config.TelecardEncryptedPassword.Decrypt(),
                SenderMask = config.SenderMask,

                DealerApiUsername = config.DealerApiUsername,
                DealerApiPassword = config.DealerApiEncryptedPassword.Decrypt()
            };

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = dto
            });
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] CredentialConfigDto dto)
        {
            var config = await _context.CredentialConfig.FirstOrDefaultAsync();

            if (config == null)
            {
                config = new CredentialConfig
                {
                    CreatedBy = dto.ModifiedBy,
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedUserName = dto.LastModifiedUserName,
                };
                _context.CredentialConfig.Add(config);
            }

            // SMTP
            config.SmtpHost = dto.SmtpHost;
            config.SmtpPort = dto.SmtpPort;
            config.SmtpUsername = dto.SmtpUsername;
            config.SmtpEncryptedPassword = dto.SmtpPassword.Encrypt();
            config.SmtpEncryptionType = dto.SmtpEncryptionType;
            config.FromEmail = dto.FromEmail;

            // Telecard
            config.TelecardApiUsername = dto.TelecardApiUsername;
            config.TelecardEncryptedPassword = dto.TelecardPassword.Encrypt();
            config.SenderMask = dto.SenderMask;

            // Dealer
            config.DealerApiUsername = dto.DealerApiUsername;
            config.DealerApiEncryptedPassword = dto.DealerApiPassword.Encrypt();

            config.ModifiedBy = dto.ModifiedBy;
            config.LastModified = DateTime.UtcNow;
            config.LastModifiedUserName = dto.LastModifiedUserName;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Configuration saved successfully",
                Data = null
            });
        }
    }
}
