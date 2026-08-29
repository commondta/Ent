using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MemberBioMetricHistery : BaseModel
    {

        public string VerificationType { get; set; }
        public int FingerId { get; set; }
        public bool IsMatched { get; set; }
        public DateTime? VerificationDateTime { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

    }
}
