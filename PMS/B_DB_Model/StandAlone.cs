using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class StandAlone : BaseModel
    {
        public bool? IsStandAloneRequested { get; set; }
        public bool? IsStandAloneApproved { get; set; }
        public bool? IsStandAloneClosed { get; set; }

        public DateTime? DocumentDate { get; set; }
        public DateTime? DueDate { get; set; }
        
        public string? RegistrationNo { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }

        [MaxLength(100)]
        public string? ChallanNo { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Type { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? CancelRemarks { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Remarks { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? PaymentMode { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? NameRecipt { get; set; } = string.Empty;
        public bool? ShowOwnerDetails { get; set; } = false;
        [MaxLength(100)]
        public string? BankAccountDD { get; set; } = string.Empty;


        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<StandAloneCharges>? StandAloneCharges { get; set; }
    }

    public class StandAloneCharges
    {
        [Key]
        public int Id { get; set; }
        public string ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? SapAccount { get; set; }
        public string? Remarks { get; set; }

        [NotMapped]
        public DateTime? DueDate { get; set; }

        [ForeignKey("StandAloneId")]
        public int? StandAloneId { get; set; }
        public StandAlone? StandAlone { get; set; }
    }
}
