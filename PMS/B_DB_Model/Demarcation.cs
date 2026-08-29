using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{

    public class Demarcation : BaseModel
    {
         [NotMapped]
        public int? GraceMonth { get; set; } 

        //Navigation

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<DemarcationFormAttachments>? DemarcationFormAttachments { get; set; }
    }

    public class DemarcationFormAttachments
    {
        [Key]
        public int Id { get; set; }

        public string Remarks { get; set; } = string.Empty;
        public string Piture { get; set; } = string.Empty;

        [ForeignKey("DemarcationId")]
        public int? DemarcationId { get; set; }
        public Demarcation? Demarcation { get; set; }
    }
}
