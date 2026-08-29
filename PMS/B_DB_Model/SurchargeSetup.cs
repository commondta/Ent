using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SurchargeSetup : BaseModel
    {
        public double? KIBOR { get; set; }
        public double? Addition { get; set; }
        public double? TotalSurCharge { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
