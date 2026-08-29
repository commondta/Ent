using B_DB_Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class GeneratePossessionCreateModel
    {
        public int fromId { get; set; }
        public int toId { get; set; }
        public DateTime possessionEffectDate { get; set; }

        public virtual ICollection<GeneratePossessionAttachmentCreateModel>? possessionFormAttachments { get; set; }

    }

    public class GeneratePossessionAttachmentCreateModel
    {
        [MaxLength(2000)]
        public string Remarks { get; set; } = string.Empty;
        public string Piture { get; set; } = string.Empty;
        public int? StockCreationId { get; set; }
    }

}
