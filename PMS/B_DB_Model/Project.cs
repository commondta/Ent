using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Code { get; set; }

        public int? RealStateTypeId { get; set; }

        public string Description { get; set; }
        public DateTime Created_at { get; set; }
        public int Created_By { get; set; }
        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }


        //DTo items

        [NotMapped]
        public string? RealStateTypeName { get; set; }
    }
}
