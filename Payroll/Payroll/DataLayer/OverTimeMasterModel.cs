using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class OverTimeMasterModel
    {
        public int id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string MaxOTHours { get; set; }
        public string MinOTHours { get; set; }
        public string Factors { get; set; }
    }
}
