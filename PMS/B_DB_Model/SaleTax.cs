using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SaleTax : BaseModel
    {
        public string? TaxCode { get; set; }
        public decimal? Rate { get; set; } = 0;
        public string? Description { get; set; }

        [MaxLength(250)]
        public string? TaxCodeElectricity { get; set; }
        public decimal? RateElectricity { get; set; } = 0;
        [MaxLength(250)]
        public string? DescriptionElectricity { get; set; }

        [MaxLength(250)]
        public string? TaxCodeNonConstructed { get; set; }
        public decimal? RateNonConstructed { get; set; } = 0;
        [MaxLength(250)]
        public string? DescriptionNonConstructed { get; set; }

        [MaxLength(250)]
        public string? TaxCodeConstructed { get; set; }
        public decimal? RateConstructed { get; set; } = 0;
        [MaxLength(250)]
        public string? DescriptionConstructed { get; set; }
    }
}
