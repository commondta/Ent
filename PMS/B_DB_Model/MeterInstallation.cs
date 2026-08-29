using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MeterInstallation : BaseModel
    {
        public string PropertyNo { get; set; } = String.Empty;
        public string Project { get; set; } = String.Empty;
        public string Remarks { get; set; } = String.Empty;

        //Navigation

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        public virtual ICollection<MeterDetail>? MeterDetail { get; set; }
    }

    public class MeterDetail : BaseModel
    {
        public string MeterNumber { get; set; } = String.Empty;
        public decimal UnitsAtInstallation  { get; set; }
        public string Status { get; set; } = String.Empty;
        public DateTime? Date { get; set; } 
        public string Remarks { get; set; } = String.Empty;

        //Navigation
        [ForeignKey("MeterTypeId")]
        public int? MeterTypeId { get; set; }
        public virtual MeterType? MeterType { get; set; }

        [ForeignKey("MeterPhaseId")]
        public int? MeterPhaseId { get; set; }
        public virtual MeterPhase? MeterPhase { get; set; }

        [ForeignKey("MeterStatusId")]
        public int? MeterStatusId { get; set; }
        public virtual MeterStatus? MeterStatus { get; set; }

        [ForeignKey("MeterInstallationId")]
        public int? MeterInstallationId { get; set; }
        public virtual MeterInstallation? MeterInstallation { get; set; }

        [NotMapped]
        public int? WTax { get; set; }
    }
}
