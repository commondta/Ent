using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SalaryDetailModel
    {
        public int id { get; set; }
        public int employeeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
        public Boolean OT { get; set; }
        public Boolean Tax { get; set; }
    }
}
