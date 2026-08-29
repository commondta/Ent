using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FileVerificationRequest : BaseModel
    {
        public bool? IsFileVerificationRequested { get; set; }
        public bool? IsFileVerificationApproved { get; set; }
        public bool? IsRequestClosed { get; set; }
        
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }

        public string? ReceivedBy { get; set; }
        public string? RecieverFatherName { get; set; }
        public string? RecieverCNIC { get; set; }
        public string? RecieverMobile { get; set; }


        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<FileVerificationRequestCharges>? FileVerificationRequestCharges { get; set; }
        public virtual ICollection<FileVerificationAttachments>? FileVerificationAttachments { get; set; }
    }

    public class FileVerificationRequestCharges 
    {
        [Key]
        public int Id { get; set; }
        public string ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? SapAccount { get; set; }

        [ForeignKey("FileVerificationRequestId")]
        public int? FileVerificationRequestId { get; set; }
        public FileVerificationRequest? FileVerificationRequest { get; set; }
    }
    public class FileVerificationAttachments : BaseModel
    {
        public string? DoucmentName { get; set; }
        public string? Document { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("FileVerificationRequestId")]
        public int? FileVerificationRequestId { get; set; }
        public FileVerificationRequest? FileVerificationRequest { get; set; }
    }
}
