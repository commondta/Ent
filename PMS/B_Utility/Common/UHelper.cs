using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static B_Utility.Common.Global_Utility;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using B_DB_Model;

namespace B_Utility.Common
{
    public static class UHelper
    {
        public static string CreateJWT(PMSUser user, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[] {
            new Claim(ClaimTypes.Name,user.Username),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
        };

            var signingCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string CreateJWTMobile(MemberProfile user, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[] {
            new Claim(ClaimTypes.Name,user.MemberName),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
        };

            var signingCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static ApiResponse<object> ApiExceptionResponse(string message)
        {
            return new ApiResponse<object>
            {
                Code = ResponseCode.Error,
                Message = message,
                Data = null
            };
        }

        public static string ApprovalStatus(int approvalStatus)
        {
            string status = "";

            switch (approvalStatus)
            {
                case 1:
                    status = "Not Assigned Yet";
                    break;
                case 2:
                    status = "Pending";
                    break;
                case 3:
                    status = "Request For Update";
                    break;
                case 4:
                    status = "Approved";
                    break;
                case 5:
                    status = "Reject";
                    break;
                default:
                    status = "OOPS! Status Values Not Matching";
                    break;
            }

            return status;
        }

        public static string ExtractIdentifierFromUrl(string url)
        {
            string filename = Path.GetFileNameWithoutExtension(url);
            return filename;
        }

        public static bool IsUrl(string input)
        {
            if (Uri.TryCreate(input, UriKind.Absolute, out Uri uri))
            {
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }
            else
            {
                return false;
            }
        }

        public static string ExtractExtensionFromBase64DataUri(string base64DataUri)
        {
            var regex = new Regex(@"data:(?<type>.*?);base64,");
            var match = regex.Match(base64DataUri);
            var type = match.Groups["type"].Value;
            var extension = "";

            switch (type)
            {
                case "image/png":
                    extension = "png";
                    break;
                case "image/jpeg":
                    extension = "jpeg";
                    break;
                case "image/jpg":
                    extension = "jpg";
                    break;
                default:
                    throw new ArgumentException("Unsupported file type");
            }

            return extension;
        }

        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("{0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("{0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("{0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("{0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        public static int GetYearsSinceDate(DateTime inputDate)
        {
            DateTime currentDate = DateTime.Today;
            int years = currentDate.Year - inputDate.Year;

            if (currentDate.Month < inputDate.Month ||
                (currentDate.Month == inputDate.Month && currentDate.Day < inputDate.Day))
            {
                years--;
            }

            return years;
        }
        public static string GenerateUniqueNumber()
        {
            Guid guid = Guid.NewGuid();
            string uniqueNumber = $"{guid.ToString().Replace("-", string.Empty)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            return uniqueNumber;
        }
    }

}
