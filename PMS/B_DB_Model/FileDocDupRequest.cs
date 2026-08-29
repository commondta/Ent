using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FileDocDupRequest : BaseModel
    {
        public bool? IsFileDocDupRequested { get; set; }
        public bool? IsFileDocDupApproved { get; set; }
        public bool? IsRequestClosed { get; set; }

        public string? RequestType { get; set; }

        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }


        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<FileDocDupRequestedCharges>? FileDocDupRequestedCharges { get; set; }
    }

    public class FileDocDupRequestedCharges
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
}

