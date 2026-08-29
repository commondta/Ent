using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class LoanApplication2Model
    {
        public int id { get; set; }
        public int tid { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
    }
}
