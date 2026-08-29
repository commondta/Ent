using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PayElementsModel
    {
        public string id { get; set; }
        public string PayElementCode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string PayElementType { get; set; }
        public int Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Taxable { get; set; }
    }
}
