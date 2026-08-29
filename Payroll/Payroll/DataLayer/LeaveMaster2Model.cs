using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Models
{
    public class LeaveMaster2
    {
        public int id { get; set; }
        public int tid { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public string Closee { get; set; }
    }
}