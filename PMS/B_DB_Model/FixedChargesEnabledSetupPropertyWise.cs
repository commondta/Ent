using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FixedChargesEnabledSetupPropertyWise : BaseModel
    {
        public int MatchId { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? TaxCode { get; set; }
        public int? Rate { get; set; }
        public int? Discount { get; set; }
        public string? Description { get; set; }
        public bool? IsEnabled { get; set; }

        //Navigation

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }
    }
}
