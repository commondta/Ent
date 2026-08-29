using B_DB_Context;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs.SMSDTO;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace HRMS_Web.Services.SMSService
{
    public class SMSService : ISMSService
    {
        private readonly HttpClient _httpClient;
        private readonly DataBase_Context _db;
        private readonly SmsApiSettings _settings;

        public SMSService(HttpClient httpClient, IOptions<SmsApiSettings> settings, DataBase_Context db)
        {
            _httpClient = httpClient;
            _db = db;
            _settings = settings.Value;
        }

        public async Task<string> SendSingleSmsAsync(string message, string mobileNumber)
        {
            var config = _db.CredentialConfig.FirstOrDefault();

            var senderMask = NormalizeSmsMask(config.SenderMask);

            var url = $"{_settings.BaseSingleApiUrl}"
                    + $"?userid={config.TelecardApiUsername.Trim()}"
                    + $"&pwd={config.TelecardEncryptedPassword.Trim().Decrypt()}"
                    + $"&msg={Uri.EscapeDataString(message)}"
                    + $"&mobileno={mobileNumber}"
                    + $"&mask={Uri.EscapeDataString(senderMask)}";

            return await SendRequestAsync(url);
        }

        private static string NormalizeSmsMask(string mask)
        {
            if (string.IsNullOrWhiteSpace(mask))
                return string.Empty;

            return Regex.Replace(
                    mask.Replace('\u00A0', ' '),  
                    @"\s+",                       
                    " "
                ).Trim();
        }


        public async Task<string> SendMultiSmsAsync(string[] mobileNumbers, string message)
        {
            var mobileNumbersString = string.Join(",", mobileNumbers);
            var url = $"{_settings.BaseMultiApiUrl}?userid={_settings.UserId}&pwd={_settings.Password}&msg={Uri.EscapeDataString(message)}&mobileno={mobileNumbersString}&mask={_settings.Mask}";
            return await SendRequestAsync(url);
        }

        private async Task<string> SendRequestAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                throw new Exception("Error sending SMS", ex);
            }
        }
    }
}
