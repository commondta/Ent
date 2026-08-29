using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class PaymentModel
    {
        public int paymentId { get; set; }
        public int employeeId { get; set; }
        public string salary { get; set; }
        public string deduction { get; set; }
        public string leaves { get; set; }
        public string workingDays { get; set; }
        public string total { get; set; }
        public string date { get; set; }
    }
}
