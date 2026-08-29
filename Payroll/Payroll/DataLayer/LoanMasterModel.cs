using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class LoanMasterModel
    {
        public int id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string LoanType { get; set; }
        public string MaxAmount { get; set; }
        public string RateofInterest { get; set; }
        public string MinRepaymentAmount { get; set; }
        public string MaxNoofInstallments { get; set; }
        public string Remarks { get; set; }
    }
}
