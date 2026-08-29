using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class WithHoldingTax : BaseModel
    {
        public string? TaxCode { get; set; }
        public int? Rate { get; set; }
        public string? Description { get; set; }
    }
}
