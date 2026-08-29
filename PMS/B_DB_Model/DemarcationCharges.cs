using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class DemarcationCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime? DocumentDate { get; set; } = DateTime.Now;
        public DateTime? EffectiveDate { get; set; } 
        public DateTime? ClosingDate { get; set; } 
    
        public string? DocumentStatus { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? Category { get; set; }
        public string? Remarks { get; set; }
     
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public bool? is_active { get; set; }
        public bool? is_deleted { get; set; }


        public IEnumerable<DemarcationChargesDetail> DemarcationChargesDetails { get; set; }

    }
    public class DemarcationChargesDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ChargeName { get; set; }
        public string? Ammount { get; set; }

        [ForeignKey("DemarcationChargeId")]
        public int? DemarcationChargeId { get; set; }
        public DemarcationCharge? DemarcationCharge { get; set; }
    }
}
