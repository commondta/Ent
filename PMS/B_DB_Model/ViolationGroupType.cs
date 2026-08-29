using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ViolationGroupType : BaseModel
    {
        public int? Code { get; set; }
        [MaxLength(500)]
        public string ViolationTypeName { get; set; }

        public int Amount { get; set; } = 0;

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [ForeignKey("ViolationGroupId")]
        public int? ViolationGroupId { get; set; }
        public ViolationGroup? ViolationGroup { get; set; }
    }
}
