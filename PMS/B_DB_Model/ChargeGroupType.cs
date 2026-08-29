using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ChargeGroupType : BaseModel
    {
        public int? Code { get; set; }
        public string ChargeTypeName { get; set; }
        public string? SapAccount { get; set; }

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [ForeignKey("GlobalChargeGroupId")]
        public int? GlobalChargeGroupId { get; set; }
        public GlobalChargeGroup? GlobalChargeGroup { get; set; }
    }
}
