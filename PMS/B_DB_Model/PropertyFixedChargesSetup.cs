using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PropertyFixedChargesSetup : BaseModel
    {
        public int MatchId { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? ChargeType { get; set; }
        public decimal Unit { get; set; }
        public decimal ChargeSetupRate { get; set; }
        public decimal Rate { get; set; }
        public int Discount { get; set; }
        public string? ChargeDes { get; set; }
        public bool? IsEnabled { get; set; }
        public int? GlobalChargeSetupId { get; set; }

        //Navigation
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }
    }
}
