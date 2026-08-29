using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MeterPhaseWiseRate : BaseModel
    {
        [ForeignKey("MeterPhaseId")]
        public int? MeterPhaseId { get; set; }
        public virtual MeterPhase? MeterPhase { get; set; }

        public decimal Rate { get; set; }
    }
}
