using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ApprovalHistery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int ApprovalUIId { get; set; }

        public string? ActionTakenByName { get; set; }

        public string? ActionTakenUserRole { get; set; }

        public DateTime ActionDateTime { get; set; }

        public string? Action { get; set; }

        public string? ActionComment { get; set; }
    }
}
