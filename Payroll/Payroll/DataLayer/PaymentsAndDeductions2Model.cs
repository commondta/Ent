using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PaymentsAndDeductions2Model
    {
        public int id { get; set; }
        public int ParentID { get; set; }
        public string PayPeriod { get; set; }
        public DateTime DocumentDate { get; set; }
        public int DocumentNo { get; set; }
        public string Status { get; set; }
        public string PayrollName { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public string PayrollPayElement { get; set; }
        public string TransactionType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Recurrence { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string Comments { get; set; }
    }
}
