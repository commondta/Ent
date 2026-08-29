using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SitePlan : BaseModel
    {
         public bool? IsSitePlanRequested { get; set; }
         public bool? IsSitePlanApproved { get; set; }
         public bool? IsRequestClosed { get; set; }

         public string? Block { get; set; }
         public string? Category { get; set; }
         public string? Size { get; set; }
         public string? PossessionStatus { get; set; }
         public string? ConstrucationStatus { get; set; }

         [ForeignKey("MemberProfileId")]
         public int? MemberProfileId { get; set; }
         public MemberProfile? MemberProfile { get; set; }

         [ForeignKey("StockCreationId")]
         public int? StockCreationId { get; set; }
         public StockCreation? StockCreation { get; set; }

         public virtual ICollection<SitePlanAttachments>? SitePlanAttachments { get; set; }
    }

    public class SitePlanAttachments : BaseModel
    {
        public string? DoucmentName { get; set; }
        public string? Document { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("SitePlanId")]
        public int? SitePlanId { get; set; }
        public SitePlan? SitePlan { get; set; }
    }
}
