using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs.OTPDTO
{
    public class SendOTP
    {
        public class SendOtpRequest
        {
            [Required]
            public string Source { get; set; }
            [Required]
            public string Cnic { get; set; }
        }
        //public class SendOtpRequest
        //{
        //    public string Destination { get; set; }   // email address or phone number
        //    public OtpChannel Channel { get; set; }   // Email or Sms
        //}

        //public class ResendOtpRequest
        //{
        //    public string Destination { get; set; }
        //    public OtpChannel Channel { get; set; }
        //}

        public class VerifyOtpRequest
        {
            [Required]
            public string Cnic { get; set; }
            [Required]
            public string Otp { get; set; }

        }

        //public class ResetPasswordRequest
        //{
        //    public string Destination { get; set; }
        //    public string OtpCode { get; set; }
        //    public string NewPassword { get; set; }
        //}

        //public enum OtpChannel
        //{
        //    Email,
        //    Sms
        //}

    }
}
