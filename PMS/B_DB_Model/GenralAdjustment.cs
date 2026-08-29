using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class GenralAdjustment : BaseModel
    {
        public bool? IsGenralAdjustmentRequested { get; set; }
        public bool? IsGenralAdjustmentApproved { get; set; }
        public bool? IsGenralAdjustmentClosed { get; set; }

        public string? InvoiceNo { get; set; }
        public DateTime? DocumentDate { get; set; }

        public string? RegistrationNo { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }
        public string? Remarks { get; set; }
       


        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<GenralAdjustmentCharges>? GenralAdjustmentCharges { get; set; }
    }

    public class GenralAdjustmentCharges
    {
        [Key]
        public int Id { get; set; }
        public string ChargeName { get; set; }

        public int? Amount { get; set; }
        public int? Adjustment { get; set; }
        public int? NetAmount { get; set; }
        public string? SapAccount { get; set; }
        [NotMapped]
        public int LineNum { get; set; }

        [ForeignKey("GenralAdjustmentId")]
        public int? GenralAdjustmentId { get; set; }
        public GenralAdjustment? GenralAdjustment { get; set; }
    }
}

