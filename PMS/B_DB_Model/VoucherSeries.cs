using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class VoucherSeries
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? VoucherType { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Prefix { get; set; } = string.Empty;

        public int CurrentNumber { get; set; }
        [MaxLength(100)]
        public string FinancialYear { get; set; } = string.Empty;
        public DateTime UpdatedOn { get; set; }
    }

}
