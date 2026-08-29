using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class JointMemberHistoricalData 
    {
        [Key]
        public int Id { get; set; }
        public int? MemberProfileId { get; set; }
        public int? StockCreationId { get; set; }
        public string? Name { get; set; }
        public string? Relationship { get; set; }
        public string? CNIC { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
    }
}
