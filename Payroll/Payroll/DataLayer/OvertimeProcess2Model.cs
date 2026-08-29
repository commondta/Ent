using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class OvertimeProcess2Model
    {
        public int id { get; set; }
        public int tid { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string CompanyID { get; set; }
        public string NetAmount { get; set; }
        public string Basic { get; set; }
        public string WeekDaysOTHours { get; set; }
        public string WeekDaysOTAmount { get; set; }
        public string WeekEndOTHours { get; set; }
        public string WeekEndOTAmount { get; set; }
        public string MIGAllowance { get; set; }
        public string GrossSalary { get; set; }
        public string OtherDeductions { get; set; }
        public string Remarks { get; set; }
    }
}
