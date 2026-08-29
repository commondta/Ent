using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PayPeriodModel
    {
        public int id { get; set; }
        public string LocationProjectSite { get; set; }
        public string PayPeriodCodeMonth { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PayMonth { get; set; }
        public int NoOfWorkingDays { get; set; }
        public int NoOfFridays { get; set; }
        public int NoOfHolidays { get; set; }
        public int MaximumNormalOTHoursMonth { get; set; }
        public int MaximumWorkingHoursMonth { get; set; }
        public string Remarks { get; set; }
    }
}
