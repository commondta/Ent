using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Dealer : BaseModel
    {
        
        public string PictureBase64 { get; set; } 
        public string? DealerStatus { get; set; }
        public string? RegistrationFee { get; set; } 
        public string? CNIC { get; set; } 
        public string? ResidentialAddress { get; set; } 
        public int DealerCategoryId { get; set; }
        public DealerCategory? DealerCategory { get; set; }
        public string? PrincipalOwner { get; set; } 
        public string? EstateName { get; set; } 
        public string? EstateAddress { get; set; } 
        public string? Email { get; set; } 
        public string? ContactNo { get; set; } 
        public DateTime RenewalDate { get; set; } 
        public string? Nationality { get; set; } 
        public string? Country { get; set; }
        public string? City { get; set; }

        public string? DelaerRegisrationCode { get; set; }

        [DataType("decimal(18,2)")]
        public double? OutstandingBalance { get; set; } 

        [DataType("decimal(18,2)")]
        public double? OutstandingAdvance { get; set; } 

        public string? UserName { get; set; } 
        public string? Password { get; set; } 

        public bool? IsDealerProfileRequested { get; set; }
        public bool? IsDealerProfileApproved{ get; set; }

        //Navigation 
        public virtual ICollection<DealerEstateDeatail>? DealerEstateDeatail { get; set; }
        public virtual ICollection<DealerAttachments>? DealerAttachments { get; set; }
        public virtual ICollection<DealerRelationshipHistery>? DealerRelationshipHistory { get; set; }
        public virtual ICollection<DealerWitness>? DealerWitness { get; set; }

    }

    public class DealerEstateDeatail : BaseModel
    {

        [ForeignKey("DealerDesignationId")]
        public int? DealerDesignationId { get; set; }
        public DealerDesignation? DealerDesignation { get; set;} 
        public string? Name { get; set; } 
        public string? CNIC { get; set; } 
        public string? MobileNo { get; set; } 
        public string? TelephoneNo { get; set; }
        public string? EmailAddress { get; set; } 
        public string? Address { get; set; } 
        public string? Remarks { get; set; }
        public string? Picture { get; set; }
        public bool? IsPrimary { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }

    }

    public class DealerRelationshipHistery : BaseModel
    {
        public DateTime? AlertDate { get; set; }
        public string? AlertNarration { get; set; } 
        public string? AlertType { get; set; } 
        public DateTime? ResolationDate { get; set; }
        public string? ResolationDescription { get; set; } 
      
        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }
    }
    public class DealerAttachments : BaseModel
    {
       
        public string? AttachmentName { get; set; } 
        public string? Attachment { get; set; } 
        public DateTime? AttachmentDate { get; set; } 
        public string? Remarks { get; set; } 

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }
    }
    public class DealerWitness : BaseModel
    {
       
        public string? Name { get; set; } 
        public string? Designation { get; set; } 
        public string? Email { get; set; } 
        public string? MobileNo { get; set; } 
        public string? Remarks { get; set; } 
        public string? CNIC { get; set; } 

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }
    }
}
