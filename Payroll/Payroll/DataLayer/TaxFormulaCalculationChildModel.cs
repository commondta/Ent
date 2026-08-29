using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TaxFormulaCalculationChildModel
    {
        public int id { get; set; }
        public int ParentId { get; set; }
        public int LowerAmount { get; set; }
        public int HigherAmount { get; set; }
        public Double Percentage { get; set; }
        public int FixedAmount { get; set; }
        public int OtherAmount { get; set; }
        public string Remarks { get; set; }
    }
}
