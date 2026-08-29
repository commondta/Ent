using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class LeadGenration : BaseModel
    {
        public string HonorificsName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string HonorificsContactPersoon { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactPersonNumber { get; set; } = string.Empty;

        public string Relationship { get; set; } = string.Empty;
        public string RelationshipWith { get; set; } = string.Empty;

        public string Cnic { get; set; } = string.Empty;
        public string CnicExpirtyDate { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string WhatsAppNo { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string CountryOfResidence { get; set; } = string.Empty;
        public string CityOfResidence { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Interst { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string SourceOfInfo { get; set; } = string.Empty;
        public string ModeOfContact { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string LGType { get; set; } = string.Empty;

        //Navigation
        public virtual ICollection<LGSocialStatus>? LGSocialStatus { get; set; }
        public virtual ICollection<LGActivities>? LGActivities { get; set; }
        public virtual ICollection<LGInterests>? LGInterests { get; set; }

    }

    public class LGSocialStatus
    {
        [Key]
        public int Id { get; set; }
        public string SocialStatus { get; set; } = string.Empty;

        [ForeignKey("LeadGenrationId")]
        public int? LeadGenrationId { get; set; }
        public LeadGenration? LeadGenration { get; set; }
    }

    public class LGActivities
    {
        [Key]
        public int Id { get; set; }
        public DateTime ContactDate { get; set; }
        public string ContactPerson { get; set; } = string.Empty;
        public string ModeOfConatcat { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public DateTime NextContactDate { get; set; }

        [ForeignKey("LeadGenrationId")]
        public int? LeadGenrationId { get; set; }
        public LeadGenration? LeadGenration { get; set; }
    }

    public class LGInterests
    {
        [Key]
        public int Id { get; set; }
        public string? PropertyNature { get; set; } 
        public string? PropertyType { get; set; } 
        public string? Category { get; set; } 

        [ForeignKey("LeadGenrationId")]
        public int? LeadGenrationId { get; set; }
        public LeadGenration? LeadGenration { get; set; }
    }
}
