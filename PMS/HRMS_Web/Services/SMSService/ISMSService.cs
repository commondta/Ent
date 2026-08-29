namespace HRMS_Web.Services.SMSService
{
    public interface ISMSService
    {
        Task<string> SendSingleSmsAsync(string mobileNumber, string message);
        Task<string> SendMultiSmsAsync(string[] mobileNumbers, string message);
    }
}
