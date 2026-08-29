using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class GLDetermination : BaseModel
    {
        public string? SapAccountCode { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("ChargeGroupTypeId")]
        public int? ChargeGroupTypeId { get; set; }
        public virtual ChargeGroupType? ChargeGroupType { get; set; }
    }
}
