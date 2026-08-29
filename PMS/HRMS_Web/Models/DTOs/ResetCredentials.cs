using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class ResetCredentials
    {
        [Required]
        public string Cnic { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
