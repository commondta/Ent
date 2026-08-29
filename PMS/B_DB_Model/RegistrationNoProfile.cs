using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class RegistrationNoProfile : BaseModel
    {
        public string? Remarks { get; set; } 
        public string? CorrespondenceAddress { get; set; } 
        public string? CorrespondenceEmail { get; set; } 
        public string? CorrespondenceMobileNo { get; set; } 
        public string? CorrespondenceWhatsappNo { get; set; } 

        //Navigation 
        [ForeignKey("StockCreationId")]
        public int StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        public virtual ICollection<SoftLock>? SoftLock { get; set; }
        public virtual ICollection<PropertyStatus>? PropertyStatus { get; set; }
        public virtual ICollection<Alerts>? Alerts { get; set; }
        public virtual ICollection<RegNoProfileAttachments>? RegNoProfileAttachments { get; set; }
    }

    public class SoftLock : BaseModel
    {
        public string SoftLockName { get; set; }
        public string Reason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        // Navigation
        [ForeignKey("RegistrationNoProfileId")]
        public int RegistrationNoProfileId { get; set; }
        public RegistrationNoProfile? RegistrationNoProfile { get; set; }
    }

    public class PropertyStatus : BaseModel
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string? Authoriser { get; set; }
        public string? Remarks { get; set; }
        // Navigation
        [ForeignKey("RegistrationNoProfileId")]
        public int RegistrationNoProfileId { get; set; }
        public RegistrationNoProfile? RegistrationNoProfile { get; set; }
    }

    public class Alerts : BaseModel
    {
        public string AlertName { get; set; } = String.Empty;
        public string AlertNarration { get; set; } = String.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string User { get; set; } = String.Empty;
        public string OrderedBy { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;

        // Navigation
        [ForeignKey("RegistrationNoProfileId")]
        public int RegistrationNoProfileId { get; set; }
        public RegistrationNoProfile? RegistrationNoProfile { get; set; }
    }

    public class RegNoProfileAttachments : BaseModel
    {
        public string AttachmentName { get; set; } = String.Empty;
        public string Attachment { get; set; } = string.Empty;
        public DateTime AttachmentDate { get; set; }
        public string Remarks { get; set; } = String.Empty;

        // Navigation
        [ForeignKey("RegistrationNoProfileId")]
        public int RegistrationNoProfileId { get; set; }
        public RegistrationNoProfile? RegistrationNoProfile { get; set; }
    }

    public class BillActivationSetupDto
    {
        public string GeneratorUnitType { get; set; } = String.Empty;
        public bool? IsBillGenerationEnabled { get; set; }
        public bool? IsSaleTaxEnabled { get; set; }
        public bool? IsWithHoldingTaxEnabled { get; set; }
        [NotMapped]
        public int MaintenceAdvanceBillPaid { get; set; } = 0;

        [NotMapped]
        public string? BillPrintRegistrationNo { get; set; } = string.Empty;
        [NotMapped]
        public string? BillPrintPropertyNo { get; set; } = string.Empty;
        [NotMapped]
        public string? BillPrintName { get; set; } = string.Empty;
        [NotMapped]
        public string? BillPrintAddress { get; set; } = string.Empty;

        public int StockCreationId { get; set; }
        public int MemberProfileId { get; set; }

    }
}
