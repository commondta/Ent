using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Models;
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;
using HRMS_Web.Extensions;
using FirebaseAdmin.Messaging;
using iTextSharp.text.pdf.security;
using static System.Net.WebRequestMethods;
using HRMS_Web.Services.SMSService;
using System.Text;
using System.Text.RegularExpressions;
using static HRMS_Web.Models.DTOs.OTPDTO.SendOTP;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberProfileController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IConfiguration _configuration;
        private readonly ISMSService _sMSService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeSpan _otpValidity = TimeSpan.FromMinutes(5);

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public MemberProfileController(DataBase_Context db, IConfiguration configuration,ISMSService sMSService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
            _configuration = configuration;
            _sMSService = sMSService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Site Integration 

        [HttpPost("/api/MemberProfile/SendOTP")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOTP([FromBody] SendOtpRequest model)
        {
            if (!TryValidateSource(model.Source, out var isPhone, out var error))
                return BadRequest(error);

            var member = await _db.MemberProfile
                                 .Where(x => !x.IsDeleted && x.Cnic == model.Cnic)
                                 .FirstOrDefaultAsync();

            if (member == null)
                return NotFound("User with this CNIC not found.");

            if (string.IsNullOrWhiteSpace(member.Mobile) && isPhone)
                return BadRequest("The provided phone number does not match our records for this CNIC.");

            if (string.IsNullOrWhiteSpace(member.EmailId) && !isPhone)
                return BadRequest("The provided email does not match our records for this CNIC.");

            if (!isPhone)
            {
                if (string.IsNullOrWhiteSpace(model.Source))
                    return BadRequest("Email is required for email verification.");

                if (!string.Equals(member.EmailId.Trim(), model.Source.Trim(), StringComparison.OrdinalIgnoreCase))
                    return BadRequest("The provided email does not match our records for this CNIC.");
            }

            else
            {
                if (string.IsNullOrWhiteSpace(model.Source))
                    return BadRequest("Phone number is required for SMS verification.");

                if (!string.Equals(member.Mobile.Trim(), model.Source.Trim(), StringComparison.OrdinalIgnoreCase))
                    return BadRequest("The provided phone number does not match our records for this CNIC.");
            }

            return await GenerateAndSendOtpAsync(model.Cnic, model.Source, isPhone);
        }

        [HttpPost("/api/MemberProfile/ResendOTP")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendOTP([FromBody] SendOtpRequest model)
        {
          
            if (!TryValidateSource(model.Source, out var isPhone, out var error))
                return BadRequest(error);

            var member = await _db.MemberProfile
                                 .Where(x => !x.IsDeleted && x.Cnic == model.Cnic)
                                 .FirstOrDefaultAsync();

            if (member == null)
                return NotFound("User with this CNIC not found.");

            if (string.IsNullOrWhiteSpace(member.Mobile) && isPhone)          
                return BadRequest("The provided phone number does not match our records for this CNIC.");

            if (string.IsNullOrWhiteSpace(member.EmailId) && !isPhone)
                return BadRequest("The provided email does not match our records for this CNIC.");

            if (!isPhone)
            {
                if (string.IsNullOrWhiteSpace(model.Source))
                    return BadRequest("Email is required for email verification.");

                if (!string.Equals(member.EmailId.Trim(), model.Source.Trim(), StringComparison.OrdinalIgnoreCase))
                    return BadRequest("The provided email does not match our records for this CNIC.");
            }

            else
            {
                if (string.IsNullOrWhiteSpace(model.Source))
                    return BadRequest("Phone number is required for SMS verification.");

                if (!string.Equals(member.Mobile.Trim(), model.Source.Trim(), StringComparison.OrdinalIgnoreCase))
                    return BadRequest("The provided phone number does not match our records for this CNIC.");
            }

            if (member.OTPExpiry.HasValue && member.OTPExpiry.Value > DateTime.UtcNow)
            {
                var wait = member.OTPExpiry.Value - DateTime.UtcNow;
                return BadRequest(
                    $"Please wait {wait.Minutes} minute(s) and {wait.Seconds} second(s) before requesting a new OTP.");
            }

            return await GenerateAndSendOtpAsync(model.Cnic, model.Source, isPhone);
        }


        [HttpPost("/api/MemberProfile/VerifyOTP")]
        [AllowAnonymous]
        public IActionResult VerifyOTP([FromBody] VerifyOtpRequest model)
        {
            if (model == null
             || string.IsNullOrWhiteSpace(model.Cnic)
             || string.IsNullOrWhiteSpace(model.Otp))
            {
                return BadRequest("CNIC, source, and OTP are required.");
            }

            var member = _db.MemberProfile
                            .Where(x => !x.IsDeleted && x.Cnic == model.Cnic)
                            .FirstOrDefault();

            if (member == null)
                return NotFound("User not found.");

            if (member.OTPExpiry < DateTime.UtcNow)
                return BadRequest("OTP has expired.");

            if (member.OTP != model.Otp.Trim())
                return BadRequest("Invalid OTP.");

            member.OTP = null;
            member.OTPExpiry = null;
            _db.SaveChanges();

            var resetJwt = GenerateTwoFactorToken(model.Cnic);

            return Ok(new
            {
                Message = "OTP Verified. Use the returned reset token to authorize your password reset.",
                ResetToken = resetJwt
            });
        }

        private bool TryValidateSource(string source, out bool isPhone, out string errorMessage)
        {
            errorMessage = null;
            isPhone = false;

            if (string.IsNullOrWhiteSpace(source))
            {
                errorMessage = "Source (phone or email) is required.";
                return false;
            }

            source = source.Trim();
            var phonePattern = @"^\+?\d{10,15}$";
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (Regex.IsMatch(source, phonePattern))
            {
                isPhone = true;
                return true;
            }
            if (Regex.IsMatch(source, emailPattern))
            {
                isPhone = false;
                return true;
            }

            errorMessage = "Source must be a valid phone number or email address.";
            return false;
        }

        private async Task<IActionResult> GenerateAndSendOtpAsync(string cnic, string destination, bool isPhone)
        {
            try
            {
                var config = _db.CredentialConfig.FirstOrDefault();
                string otp = new Random().Next(100000, 999999).ToString();

                if (isPhone)
                {
                    var body = $"🔒 Your OTP is {otp}. Expires in 2 min. Don’t share it with anyone.";

                    await _sMSService.SendSingleSmsAsync(body,destination);
                }
                else
                {
                    var mail = new MailMessage
                    {
                        From = new MailAddress(config.FromEmail.Trim()),
                        Subject = "🔐 Your One‑Time Password (OTP) Verification",
                        IsBodyHtml = true,
                        Body = $@"
                                 <div style='font-family:Arial,sans-serif;color:#333;max-width:600px;margin:auto;padding:20px;
                                             border:1px solid #ccc;border-radius:10px;background-color:#f9f9f9'>
                                     <h2 style='text-align:center;color:#007bff;'>Your OTP: {otp}</h2>
                                     <p>Dear Valued User,</p>
                                     <p>Thank you for choosing us! Your OTP for verification is shown above.</p>
                                     <p>Please do not share it with anyone. If you did not request this, ignore this mail.</p>
                                     <p style='text-align:center;color:#777;margin-top:20px;'>Warm regards,<br/>N-Stack</p>
                                 </div>"
                    };

                    mail.To.Add(destination);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    using (var client = new SmtpClient(config.SmtpHost, (int)config.SmtpPort))
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(
                            config.SmtpUsername.Trim(),
                            config.SmtpEncryptedPassword.Decrypt()
                        );
                        client.EnableSsl = true; 
                        client.Send(mail);
                    }

                }

                var member = _db.MemberProfile
                                .Where(x => !x.IsDeleted && x.Cnic == cnic)
                                .FirstOrDefault();

                if (member == null)
                    return NotFound("User not found.");

                member.OTP = otp;
                member.OTPExpiry = DateTime.UtcNow.Add(_otpValidity);
                _db.SaveChanges();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok("OTP sent successfully.");
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("/api/MemberProfile/GetUserByCnic")]
        public IActionResult GetUserByCnic(string cnic)
        {
            try
            {

                var result = _db.MemberProfile.Where(x => !x.IsDeleted && x.Cnic == cnic)
                                              .Select(x => new
                                              {
                                                  x.Cnic,
                                                  x.EmailId,
                                                  x.Mobile
                                              })
                                              .FirstOrDefault();
                if (result == null)
                {
                    return NotFound(new { Message = "not found." });
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "TwoFactorScheme", Policy = "Require2FAScope")]
        [Route("/api/MemberProfile/ResetCredentials")]
        public IActionResult RestPassword([FromBody] ResetCredentials model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var cnic = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
             ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(cnic) || cnic != model.Cnic)
                    return Unauthorized("Invalid reset token.");

                var data = _db.MemberProfile.Where(x => x.Cnic == model.Cnic).FirstOrDefault();

                if (data != null && data.UserName == model.UserName)
                {
                    return BadRequest("Member with UserName Already Exist! Please try With Another UserName");
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    byte[] passwordHashing, passwordKey;

                    using (var hmac = new HMACSHA512())
                    {
                        passwordKey = hmac.Key;
                        passwordHashing = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));

                    }

                    data.UserName = model.UserName;
                    data.PasswordHash = passwordHashing;
                    data.PasswordKey = passwordKey;
                    data.Password = null;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return Ok("Success");
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private string GenerateTwoFactorToken(string cnic)
        {
            var cfg = _configuration.GetSection("TwoFactorJwtSettings");
            var key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(cfg["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, cnic),
                new Claim("scope", "2fa")
            };

            var expiry = DateTime.UtcNow.AddMinutes(
                             int.Parse(cfg["ExpiryMinutes"]));

            var jwt = new JwtSecurityToken(
                issuer: cfg["Issuer"],
                audience: cfg["Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


        private string GenerateResetToken(string cnic)
        {
            var resetCfg = _configuration.GetSection("ResetJwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(resetCfg["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, cnic),
        new Claim("scope", "reset")
    };

            var expiry = DateTime.UtcNow.AddMinutes(
                             int.Parse(resetCfg["ExpiryMinutes"]));

            var token = new JwtSecurityToken(
                issuer: resetCfg["Issuer"],
                audience: resetCfg["Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("/api/MemberProfile/SignIn")]
        public IActionResult SignIn([FromBody] LoginRequestModel model)
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted && x.UserName == model.UserName && x.IsActive == true)
                                              .FirstOrDefault();

                if (result == null)
                {
                    return NotFound(new { Message = "not found." });
                }

                if (!MatchPasswordHash(model.Password, result.PasswordHash, result.PasswordKey))
                {
                    return BadRequest("invalid password");
                }
                else
                {
                    string token = UHelper.CreateJWTMobile(result, _configuration);
                    HttpContext.Session.SetString("ID", result.Id.ToString());
                    HttpContext.Session.SetString("FullName", result.MemberName);
                    HttpContext.Session.SetString("token", token);

                    return Ok(new
                    {
                        Name = result.MemberName ?? "N/A",
                        Token = token ?? "No Token Provided"
                    });
                }
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
            }
        }


        [HttpGet]
        [Route("/api/MemberProfile/GetMemberDetail")]
        [ValidateSession]
        public IActionResult GetMemberDetail()
        {
            try
            {
                var id = HttpContext.Session.GetString("ID");

                var result = _db.MemberProfile.Where(x => !x.IsDeleted && x.Id == Convert.ToInt32(id))
                               .Select(x => new
                               {
                                   MembershipNo = x.MEMBERSHIPNO,
                                   x.HonorificsName,
                                   x.MemberName,
                                   x.Relationship,
                                   x.RelationshipWith,
                                   x.MemberStatus,
                                   DOB = x.DOB,
                                   x.Gender,
                                   x.Cnic,
                                   x.CnicExpiryDate,
                                   x.PassportNo,
                                   x.PassportExpiryDate,
                                   x.Nationality,
                                   x.OverSeas,
                                   x.CountryOfResidence,
                                   x.CityOfResidence,
                                   x.SourceOfInfo,
                                   NICOPNO = x.NICOPNo,
                                   POCNO = x.POCNO,
                                   x.CurrentAddress,
                                   x.ResidenenceStatus,
                                   x.PermanentAddress,
                                   x.Mobile,
                                   x.Phone,
                                   x.EmailId,
                                   x.Profession,
                                   x.BussinessAddress,
                                   x.BussinessTenoure,
                                   x.Salary,
                                   x.TaxStatus,
                                   NTNNO = x.NTNNo,
                                   x.Password,
                                   x.UserName,
                                   x.IsMemberProfileApproved,
                                   x.MemberCategory,
                                   PANO = x.PANO,
                                   x.Rank,
                                   x.Shaheed,
                                   x.Quota,
                                   x.PrfixMembershipNo,
                                   x.RepresentativeCnic,
                                   x.RepresentativeName,
                                   x.RepresentativeRelationshipWith
                               })
                               .FirstOrDefault();


                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("/api/MemberProfile/GetMemberStock")]
        [ValidateSession]
        public IActionResult GetMemberStock()
        {
            try
            {
                var id = HttpContext.Session.GetString("ID");

                string cnic = _db.MemberProfile.Where(x=>x.Id == Convert.ToInt32(id) && !string.IsNullOrEmpty(x.Cnic)).FirstOrDefault().Cnic;

                var result = (from stock in _db.StockCreations
                              where stock.is_active == true
                                    && stock.Is_StockCreationApproved == true
                                    && stock.MemberProfile.Cnic == cnic
                              select new
                              {
                                  stock.ID,
                                  ReferenceNo = stock.RegistrationNo,
                                  PlotNo = stock.PropertyNo,
                                  Area = $"{stock.ActualSize} {stock.ActualSizeUnit}",
                                  RealStateTypeName = _db.Real_Estates
                                                         .Where(r => r.ID == Convert.ToInt32(stock.RealStateType))
                                                         .Select(r => r.Description)
                                                         .FirstOrDefault() ?? "N/A",
                                  ProjectDescription = _db.Projects
                                                          .Where(p => p.ID == Convert.ToInt32(stock.Project))
                                                          .Select(p => p.Description)
                                                          .FirstOrDefault() ?? "N/A",
                                  PhaseDescription = _db.Phases
                                                         .Where(ph => ph.ID == Convert.ToInt32(stock.Phase))
                                                         .Select(ph => ph.Description)
                                                         .FirstOrDefault() ?? "N/A",
                                  CategoryDescription = _db.Categories
                                                           .Where(c => c.ID == Convert.ToInt32(stock.Category))
                                                           .Select(c => c.Description)
                                                           .FirstOrDefault() ?? "N/A",
                                  BlockDescription = _db.Blocks
                                                         .Where(b => b.ID == Convert.ToInt32(stock.Block))
                                                         .Select(b => b.Description)
                                                         .FirstOrDefault() ?? "N/A",
                                  NatureDescription = _db.Natures
                                                          .Where(n => n.ID == Convert.ToInt32(stock.Nature))
                                                          .Select(n => n.Description)
                                                          .FirstOrDefault() ?? "N/A",
                                  TypeDescription = _db.PropertyTypes
                                                        .Where(t => t.ID == Convert.ToInt32(stock.Type))
                                                        .Select(t => t.Description)
                                                        .FirstOrDefault() ?? "N/A"
                              }).ToList();


                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("/api/MemberProfile/GetMemberPlotById")]
        [ValidateSession]
        public IActionResult GetMemberPlotById(int id)
        {
            var idMember = HttpContext.Session.GetString("ID");
            var result = (from stock in _db.StockCreations
                          where stock.ID == id && stock.MemberProfileId == Convert.ToInt32(idMember)
                          select new
                          {
                              RealStateTypeName = _db.Real_Estates
                                                         .Where(r => r.ID == Convert.ToInt32(stock.RealStateType))
                                                         .Select(r => r.Description)
                                                         .FirstOrDefault() ?? "N/A",
                              ProjectDescription = _db.Projects
                                                          .Where(p => p.ID == Convert.ToInt32(stock.Project))
                                                          .Select(p => p.Description)
                                                          .FirstOrDefault() ?? "N/A",
                              PhaseDescription = _db.Phases
                                                         .Where(ph => ph.ID == Convert.ToInt32(stock.Phase))
                                                         .Select(ph => ph.Description)
                                                         .FirstOrDefault() ?? "N/A",
                               CategoryDescription = _db.Categories
                                                           .Where(c => c.ID == Convert.ToInt32(stock.Category))
                                                           .Select(c => c.Description)
                                                           .FirstOrDefault() ?? "N/A",
                              BlockDescription = _db.Blocks
                                                         .Where(b => b.ID == Convert.ToInt32(stock.Block))
                                                         .Select(b => b.Description)
                                                         .FirstOrDefault() ?? "N/A",
                              NatureDescription = _db.Natures
                                                          .Where(n => n.ID == Convert.ToInt32(stock.Nature))
                                                          .Select(n => n.Description)
                                                          .FirstOrDefault() ?? "N/A",
                              TypeDescription = _db.PropertyTypes
                                                        .Where(t => t.ID == Convert.ToInt32(stock.Type))
                                                        .Select(t => t.Description)
                                                        .FirstOrDefault() ?? "N/A",
                              Finishing = stock.Finishing ?? "N/A",
                              Floor = stock.Floor ?? "N/A",
                              ActualSize = stock.ActualSize ?? "N/A",
                              ActualSizeUnit = stock.ActualSizeUnit ?? "N/A",
                              Status = stock.Status ?? "N/A",
                              RegistrationNo = stock.RegistrationNo ?? "N/A",
                              PropertyNo = stock.PropertyNo ?? "N/A",
                              ConstructionStatus = stock.ConstracutionStatus ?? "N/A",
                              Location = stock.Location ?? "N/A",
                              Street = stock.Street ?? "N/A",
                              Dealer = stock.Dealer.EstateName ?? "N/A",
                              CoveredArea = stock.coveredArea ?? 0,
                              InventoryStatus = stock.InventoryStatus ?? "N/A",
                              Almt = stock.Almt ?? "N/A",
                              Feature = stock.Feature ?? "N/A",
                              Latitude = stock.Latitude ?? "N/A",
                              Longitude = stock.Longitude ?? "N/A",
                              PropertyStatus = stock.PropertyStatus ?? "N/A",
                              CaseCode = stock.CaseCode ?? "N/A"
                          }).FirstOrDefault();

            if (result == null)
                return NotFound(new { Message = "Stock not found." });

            return Ok(result);
        }

        [HttpGet]
        [Route("/api/MemberProfile/SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("ID");
            HttpContext.Session.Remove("FullName");
            HttpContext.Session.Remove("token");

            return Ok();
        }

        #endregion


        [HttpGet]
        [Route("/api/MemberProfile/GetAllMembersName")]
        public IActionResult GetAllMembersName()
        {
            try
            {
                var result = _db.MemberProfile
                                        .Select(x => new
                                        {
                                            Id = x.Id,
                                            Name = x.MemberName
                                        })
                                        .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        [HttpGet]
        [Route("/api/MemberProfile/GetAllCountries")]
        public IActionResult GetAllCountries()
        {
            try
            {
                var result = _db.MemberProfile
                                        .Select(x => new
                                        {
                                            Name = x.CountryOfResidence
                                        })
                                        .ToList()
                                        .DistinctBy(x => x.Name);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        [HttpGet]
        [Route("/api/MemberProfile/GetAllCities")]
        public IActionResult GetAllCities(string country)
        {
            try
            {
                var result = _db.MemberProfile.Where(x=>x.CountryOfResidence == country)
                                        .Select(x => new
                                        {
                                            Name = x.CityOfResidence
                                        })
                                        .ToList()
                                        .DistinctBy(x => x.Name);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("/api/MemberProfile/GetAllQoutas")]
        public IActionResult GetAllQoutas()
        {
            try
            {
                var result = _db.MemberProfile
                                        .Select(x => new
                                        {
                                            Name = x.Quota
                                        })
                                        .Where(x=> !string.IsNullOrEmpty(x.Name))
                                        .ToList()
                                        .DistinctBy(x => x.Name);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("/api/MemberProfile/GetFilterListForRegistionNoProfile")]
        public IActionResult GetFilterListForRegistionNoProfile()
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted
                                                     )
                                               .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        
        [HttpGet]
        [Route("GetMemberById")]
        public IActionResult GetMemberById(int id)
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted && x.Id == id)
                                              .FirstOrDefault();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted)
                                                       //.Include(x => x.MemberSocialStatus.Where(x => !x.IsDeleted))
                                                       //.Include(x => x.MemberInterest.Where(x => !x.IsDeleted))
                                                       //.Include(x => x.MemberRelationshipHistory.Where(x => !x.IsDeleted))
                                                       //.Include(x => x.MemberAttachments.Where(x => !x.IsDeleted))
                                                       //.Include(x => x.MemberNominees.Where(x => !x.IsDeleted))
                                                       .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                
                var result = _db.MemberProfile.Where(x => !x.IsDeleted && x.Id == id)
                                                            .Include(x => x.MemberSocialStatus.Where(x => !x.IsDeleted))
                                                            .Include(x => x.MemberInterest.Where(x => !x.IsDeleted))
                                                            .Include(x => x.MemberRelationshipHistory.Where(x => !x.IsDeleted))
                                                            .Include(x => x.MemberAttachments.Where(x => !x.IsDeleted))
                                                            .Include(x => x.MemberNominees.Where(x => !x.IsDeleted))
                                               .FirstOrDefault();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("/api/MemberProfile/AddNewMemberProfile")]
        public async Task<IActionResult> AddNewMemberProfileAsync(MemberProfile model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.MemberRegistrationForm);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.MemberRegistrationForm).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                var data = _db.MemberProfile.Where(x => x.MEMBERSHIPNO == model.MEMBERSHIPNO).FirstOrDefault();

                if (data != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "Member with MEMBERSHIPNO Already Exist",
                        Data = null
                    });
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    byte[] passwordHashing, passwordKey;

                    using (var hmac = new HMACSHA512())
                    {
                        passwordKey = hmac.Key;
                        passwordHashing = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));

                    }

                    model.PasswordHash = passwordHashing;
                    model.PasswordKey = passwordKey;
                    model.Password = null;
                }

                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                model.ImageURL = string.IsNullOrEmpty(model.ImageURL) ? "" : $"{path}{await model.ImageURL.SaveBase64FileAsync()}";

                model.CNICFront = string.IsNullOrEmpty(model.CNICFront) ? "" : $"{path}{await model.CNICFront.SaveBase64FileAsync()}";

                model.CNICBack = string.IsNullOrEmpty(model.CNICBack) ? "" : $"{path}{await model.CNICBack.SaveBase64FileAsync()}";

                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;

                if (model.MemberSocialStatus?.Count > 0)
                {
                    foreach (var item in model.MemberSocialStatus)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }
                
                if (model.MemberInterest?.Count > 0)
                {
                    foreach (var item in model.MemberInterest)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }
                if (model.MemberRelationshipHistory?.Count > 0)
                {
                    foreach (var item in model.MemberRelationshipHistory)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.MemberNominees?.Count > 0)
                {
                    foreach (var item in model.MemberNominees)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.MemberAttachments?.Count > 0)
                {
                    foreach (var item in model.MemberAttachments)
                    {
                        item.CopyToTargetDocument = string.IsNullOrEmpty(item.CopyToTargetDocument) ? "" : $"{path}{await item.CopyToTargetDocument.SaveBase64FileAsync()}";
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.MemberProfile.Add(model);
                _db.SaveChanges();
               
                string message = string.Empty;

                MemberProfile memberProfile = (MemberProfile)_db.MemberProfile.Where(x => x.Id == model.Id)
                                                                               .FirstOrDefault();
                if (memberProfile != null)
                {
                    memberProfile.IsMemberProfileRequested = true;
                    _db.SaveChanges();
                    
                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.MemberRegistrationForm);
                        message = "Member Profile added succesfully and moved for approval";
                        if (result)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                                Data = null
                            });
                        }
                    }
                    else
                    {
                        memberProfile.IsMemberProfileApproved = true;
                        _db.SaveChanges();

                        SapIntegrationController sapIntegrationController = new SapIntegrationController(_db);
                        Response_Result sapResult = sapIntegrationController.MemberPosting(memberProfile);
                        if (sapResult.code == 0)
                        {
                            message = sapResult.message;
                        }

                        message = "Member Profile added succesfully " + message;

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("/api/MemberProfile/UpdateMemberProfile")]
        public async Task<IActionResult> UpdateMemberProfileAsync(MemberProfile model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var existing = _db.MemberProfile.Where(x => x.MEMBERSHIPNO == model.MEMBERSHIPNO && x.Id != model.Id).FirstOrDefault();

                if (existing != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "Member with MEMBERSHIPNO Already Exist",
                        Data = null
                    });
                }

                var memberProfile = _db.MemberProfile.Find(model.Id);
                memberProfile.MEMBERSHIPNO = model.MEMBERSHIPNO;

                var datas = _db.MemberProfile.Where(x => x.Cnic == memberProfile.Cnic).ToList();

                foreach (var data in datas)
                {

                    if (data != null)
                    {
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            byte[] passwordHashing, passwordKey;

                            using (var hmac = new HMACSHA512())
                            {
                                passwordKey = hmac.Key;
                                passwordHashing = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));

                            }
                            data.PasswordHash = passwordHashing;
                            data.PasswordKey = passwordKey;
                            data.Password = null;
                        }

                        if (model.ImageURL != data.ImageURL)
                        {
                            if (!string.IsNullOrEmpty(data.ImageURL))
                            {
                                data.ImageURL.DeleteFile();
                            }

                            if (!string.IsNullOrEmpty(model.ImageURL))
                            {
                                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                                data.ImageURL = $"{path}{await model.ImageURL.SaveBase64FileAsync()}";
                            }
                            else
                            {
                                data.ImageURL = "";
                            }
                        }

                        if (model.CNICFront != data.CNICFront)
                        {
                            if (!string.IsNullOrEmpty(data.CNICFront))
                            {
                                data.CNICFront.DeleteFile();
                            }

                            if (!string.IsNullOrEmpty(model.CNICFront))
                            {
                                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                                data.CNICFront = $"{path}{await model.CNICFront.SaveBase64FileAsync()}";
                            }
                            else
                            {
                                data.CNICFront = "";
                            }
                        }

                        if (model.CNICBack != data.CNICBack)
                        {
                            if (!string.IsNullOrEmpty(data.CNICBack))
                            {
                                data.CNICBack.DeleteFile();
                            }

                            if (!string.IsNullOrEmpty(model.CNICBack))
                            {
                                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                                data.CNICBack = $"{path}{await model.CNICBack.SaveBase64FileAsync()}";
                            }
                            else
                            {
                                data.CNICBack = "";
                            }
                        }


                        data.Rank = model.Rank;
                        data.Force = model.Force;
                        data.MemberCategory = model.MemberCategory;
                        data.PANO = model.PANO;
                        data.Shaheed = model.Shaheed;
                        data.HonorificsName = model.HonorificsName;
                        data.MemberName = model.MemberName;
                        data.Relationship = model.Relationship;
                        data.RelationshipWith = model.RelationshipWith;
                        data.MemberStatus = model.MemberStatus;
                        data.DOB = model.DOB;
                        data.Gender = model.Gender;
                        data.Cnic = model.Cnic;
                        data.CnicExpiryDate = model.CnicExpiryDate;
                        data.PassportNo = model.PassportNo;
                        data.PassportExpiryDate = model.PassportExpiryDate;
                        data.Nationality = model.Nationality;
                        data.OverSeas = model.OverSeas;
                        data.CountryOfResidence = model.CountryOfResidence;
                        data.CityOfResidence = model.CityOfResidence;
                        data.SourceOfInfo = model.SourceOfInfo;
                        data.OutstandingBalance = model.OutstandingBalance;
                        data.NICOPNo = model.NICOPNo;
                        data.POCNO = model.POCNO;
                        data.BioMetircInfo = model.BioMetircInfo;
                        data.Quota = model.Quota;
                        data.CurrentAddress = model.CurrentAddress;
                        data.ResidenenceStatus = model.ResidenenceStatus;
                        data.PermanentAddress = model.PermanentAddress;
                        data.Vehicle = model.Vehicle;
                        data.MothersMaidenName = model.MothersMaidenName;
                        data.Mobile = model.Mobile;
                        data.Phone = model.Phone;
                        data.HomeNo = model.HomeNo;
                        data.WhatsAppNo = model.WhatsAppNo;
                        data.OfficeNo = model.OfficeNo;
                        data.ImoNo = model.ImoNo;
                        data.EmailId = model.EmailId;
                        data.FacebookId = model.FacebookId;
                        data.InstagramId = model.InstagramId;
                        data.LinkedInId = model.LinkedInId;
                        data.TwitterId = model.TwitterId;
                        data.Profession = model.Profession;
                        data.BussinessAddress = model.BussinessAddress;
                        data.BussinessTenoure = model.BussinessTenoure;
                        data.Salary = model.Salary;
                        data.TaxStatus = model.TaxStatus;
                        data.NoOfDepartments = model.NoOfDepartments;
                        data.RelationshipManager = model.RelationshipManager;
                        data.NTNNo = model.NTNNo;
                        data.UserName = model.UserName;
                        data.ModifiedBy = model.ModifiedBy;
                        data.LastModifiedUserName = model.LastModifiedUserName;
                        data.LastModified = DateTime.Now;

                        _db.Entry(data).State = EntityState.Modified;
                        _db.SaveChanges();
                        if (data.SapPosting == true)
                        {
                            SapIntegrationController sapIntegrationController = new SapIntegrationController(_db);
                            Response_Result sapResult = sapIntegrationController.MemberUpdate(data);

                        }


                        var result = _db.MemberSocialStatus.Where(x => x.MemberProfileId == data.Id).ToList();

                        _db.MemberSocialStatus.RemoveRange(result);

                        if (model.MemberSocialStatus?.Count > 0)
                        {
                            foreach (var item in model.MemberSocialStatus)
                            {
                                item.Id = 0;
                                item.MemberProfileId = data.Id;
                                item.ModifiedBy = model.ModifiedBy;
                                item.LastModifiedUserName = model.LastModifiedUserName;
                                item.LastModified = DateTime.Now;
                                item.IsActive = true;
                                item.IsDeleted = false;
                            }

                            _db.MemberSocialStatus.AddRange(model.MemberSocialStatus);

                        }


                        var result2 = _db.MemberInterest.Where(x => x.MemberProfileId == data.Id).ToList();

                        _db.MemberInterest.RemoveRange(result2);

                        if (model.MemberInterest?.Count > 0)
                        {
                            foreach (var item in model.MemberInterest)
                            {
                                item.Id = 0;
                                item.MemberProfileId = data.Id;
                                item.ModifiedBy = model.ModifiedBy;
                                item.LastModifiedUserName = model.LastModifiedUserName;
                                item.LastModified = DateTime.Now;
                                item.IsActive = true;
                                item.IsDeleted = false;
                            }

                            _db.MemberInterest.AddRange(model.MemberInterest);

                        }


                        var result3 = _db.MemberNominees.Where(x => x.MemberProfileId == data.Id).ToList();

                        _db.MemberNominees.RemoveRange(result3);


                        if (model.MemberNominees?.Count > 0)
                        {
                            foreach (var item in model.MemberNominees)
                            {
                                item.Id = 0;
                                item.MemberProfileId = data.Id;
                                item.ModifiedBy = model.ModifiedBy;
                                item.LastModifiedUserName = model.LastModifiedUserName;
                                item.LastModified = DateTime.Now;
                                item.IsActive = true;
                                item.IsDeleted = false;
                            }

                            _db.MemberNominees.AddRange(model.MemberNominees);

                        }


                        var result4 = _db.MemberRelationshipHistery.Where(x => x.MemberProfileId == data.Id).ToList();

                        _db.MemberRelationshipHistery.RemoveRange(result4);


                        if (model.MemberRelationshipHistory?.Count > 0)
                        {
                            foreach (var item in model.MemberRelationshipHistory)
                            {   
                                item.Id = 0;
                                item.MemberProfileId = data.Id;
                                item.ModifiedBy = model.ModifiedBy;
                                item.LastModifiedUserName = model.LastModifiedUserName;
                                item.LastModified = DateTime.Now;
                                item.IsActive = true;
                                item.IsDeleted = false;
                            }

                            _db.MemberRelationshipHistery.AddRange(model.MemberRelationshipHistory);

                        }


                        var attachmentresult = _db.MemberAttachments.Where(x => x.MemberProfileId == model.Id).ToList();

                        foreach (var attachment in attachmentresult)
                        {
                            var existingFilePath = attachment.CopyToTargetDocument;

                            bool fileExistsInNewModel = model.MemberAttachments.Any(x => x.CopyToTargetDocument == existingFilePath);

                            if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                            {
                                existingFilePath.DeleteFile();
                            }

                            _db.MemberAttachments.Remove(attachment);
                        }

                        if (model.MemberAttachments?.Count > 0)
                        {
                            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                            foreach (var item in model.MemberAttachments)
                            {
                                if (!string.IsNullOrEmpty(item.CopyToTargetDocument))
                                {
                                    var savedPath = await item.CopyToTargetDocument.SaveBase64FileAsync();

                                    if (!savedPath.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
                                    {
                                        item.CopyToTargetDocument = $"{baseUrl}{savedPath}";
                                    }
                                    else
                                    {
                                        item.CopyToTargetDocument = savedPath;
                                    }
                                }
                                else
                                {
                                    item.CopyToTargetDocument = "";
                                }

                                item.MemberProfileId = data.Id;
                                item.ModifiedBy = model.ModifiedBy;
                                item.LastModifiedUserName = model.LastModifiedUserName;
                                item.LastModified = DateTime.Now;
                                item.IsActive = true;
                                item.IsDeleted = false;
                            }

                            _db.MemberAttachments.AddRange(model.MemberAttachments);

                        }

                        _db.SaveChanges();

                        //bool updated = _approvalBLL.UpdateRequestApprovalSetup(model.Id, (int)ApprovalUIIds.DealerRegistrationForm);

                        //if (updated)
                        //{
                        //    data.IsMemberProfileRequested = true;
                        //    data.IsMemberProfileApproved = false;

                        //    _db.SaveChanges();

                        //}

                    }

                    else
                    {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = null
                    });
                }
            }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteMemberProfile")]
        public IActionResult DeleteMemberProfile(int id)
        {
            try
            {
                var model = _db.MemberProfile.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var memberSocialStatuses = _db.MemberSocialStatus.Where(x => x.MemberProfileId == model.Id).ToList();

                    if (memberSocialStatuses?.Count > 0)
                    {
                        foreach (var item in memberSocialStatuses)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var memberInterests = _db.MemberInterest.Where(x => x.MemberProfileId == model.Id).ToList();

                    if (memberInterests?.Count > 0)
                    {
                        foreach (var item in memberInterests)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var memberNominees = _db.MemberNominees.Where(x => x.MemberProfileId == model.Id).ToList();

                    if (memberNominees?.Count > 0)
                    {
                        foreach (var item in memberNominees)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var memberAttachments = _db.MemberAttachments.Where(x => x.MemberProfileId == model.Id).ToList();

                    if (memberAttachments?.Count > 0)
                    {
                        foreach (var item in memberAttachments)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var memberRelationshipHistories = _db.MemberRelationshipHistery.Where(x => x.MemberProfileId == model.Id).ToList();

                    if (memberRelationshipHistories?.Count > 0)
                    {
                        foreach (var item in memberRelationshipHistories)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("MigrateMemberImagesStreaming")]
        public async Task<IActionResult> MigrateMemberImagesStreaming()
        {
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var connectionString = _db.Database.GetDbConnection().ConnectionString;

                int batchSize = 10;
                int totalProcessed = 0;

                while (true)
                {
                    var records = await GetNextBatch(connectionString, batchSize);

                    if (records.Count == 0)
                        break;

                    // 🔥 Parallel processing (SAFE)
                    await Parallel.ForEachAsync(records, new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 5 // tune (3–10)
                    },
                    async (record, ct) =>
                    {
                        try
                        {
                            var relativePath = await record.Base64.SaveBase64FileAsync();
                            var fullUrl = $"{baseUrl}{relativePath}";

                            await UpdateImageUrl(connectionString, record.Id, fullUrl);
                        }
                        catch
                        {
                            // optional: log error per record
                        }
                    });

                    totalProcessed += records.Count;

                    // small delay to reduce DB pressure
                    await Task.Delay(100);
                }

                return Ok($"Migration Completed. Total Processed: {totalProcessed}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<List<(int Id, string Base64)>> GetNextBatch(string connectionString, int batchSize)
        {
            var records = new List<(int, string)>();

            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = $@"
        SELECT TOP ({batchSize}) Id, Document
        FROM TransferSetReceivingAttachments WITH (UPDLOCK, READPAST)
        WHERE Document LIKE 'data:%'";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 600;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                records.Add((reader.GetInt32(0), reader.GetString(1)));
            }

            return records;
        }

        private async Task UpdateImageUrl(string connectionString, int id, string url)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string updateQuery = @"
        UPDATE TransferSetReceivingAttachments
        SET Document = @url
        WHERE Id = @id";

            using SqlCommand cmd = new SqlCommand(updateQuery, conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@url", url);

            cmd.CommandTimeout = 600;

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
