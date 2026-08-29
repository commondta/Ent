using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Real_Estate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        // public int? Phase { get; set; }
        [Required]
        public string Description { get; set; }

        public int? PhaseId { get; set; }

        public DateTime Created_at { get; set; }
        public int Created_By { get; set; }
        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }

        //DTo items

        [NotMapped]
        public string? PhaseName { get; set; }

    }
}
