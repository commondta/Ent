using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PayrollProcessChildModel
    {
        public int id { get; set; }
        public int ParentID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public float IncomeTax { get; set; }
        public float TotalDeduction { get; set; }
        public float NetSalary { get; set; }
        
    }
}
