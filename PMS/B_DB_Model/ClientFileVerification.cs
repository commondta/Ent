using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ClientFileVerification : BaseModel
    {
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        public bool? IsClientFileVerificationRequested { get; set; }
        public bool? IsClientFileVerificationApproved { get; set; }
        public bool? SendForApproval { get; set; }
        public string? ImageURL { get; set; }
        public string? ReceivedBy { get; set; }
        public string? RequestType { get; set; }
        public string? RecieverFatherName { get; set; }
        public string? RecieverCNIC { get; set; }
        public string? RecieverMobile { get; set; }
        public bool? IsPrintEnabled { get; set; }
        public bool? IsFilePrint { get; set; }
        public string? Remarks { get; set; }

        //dto
        [NotMapped]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string? BlockName { get; set; }
        [NotMapped]
        public int PreviousRecordId { get; set; }

        public virtual ICollection<ClientFileVerificationAttachments>? ClientFileVerificationAttachments { get; set; }
    }

    public class ClientFileVerificationAttachments
    {
        [Key]
        public int Id { get; set; }
        public string DoucmentName { get; set; }
        public string Document { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("ClientFileVerificationId")]
        public int? ClientFileVerificationId { get; set; }
        public ClientFileVerification? ClientFileVerification { get; set; }
    }
}

