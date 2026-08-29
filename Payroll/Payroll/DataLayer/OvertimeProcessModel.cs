using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class OvertimeProcessModel
    {
        public int id { get; set; }
        public string PayPeriod { get; set; }
        public string PayMonth { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Status { get; set; }
    }
}
