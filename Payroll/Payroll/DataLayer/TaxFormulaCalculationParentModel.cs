using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TaxFormulaCalculationParentModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Code { get; set; }
        public string DocumentDate { get; set; }
        public List<TaxFormulaCalculationChildModel> Child { get; set; }
    }
}
