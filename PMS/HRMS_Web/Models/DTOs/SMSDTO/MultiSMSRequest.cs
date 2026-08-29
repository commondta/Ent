namespace HRMS_Web.Models.DTOs.SMSDTO
{
    public class MultiSMSRequest
    {
        public string Message { get; set; }
        public string[] MobileNumbers { get; set; }
    }
}
