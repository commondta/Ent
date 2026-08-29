using B_DB_Model;

namespace HRMS_Web.Models.DTOs
{
    public class CredentialConfigDto
    {
        // SMTP
        public string SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpEncryptionType { get; set; }
        public string FromEmail { get; set; }

        // Telecard
        public string TelecardApiUsername { get; set; }
        public string TelecardPassword { get; set; }
        public string SenderMask { get; set; }

        // Dealer API
        public string DealerApiUsername { get; set; }
        public string DealerApiPassword { get; set; }

        // Mapping
        public int Id { get; set; }
        public int? ModifiedBy { get; set; }
        public string? LastModifiedUserName { get; set; }
    }
}
