using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PropertyList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string PropertyNo { get; set; }
        public string RegistrationNo { get; set; }
        [Required]
        public string Description { get; set; }
        public string Block { get; set; }
        public string Size { get; set; }
        public string Phase { get; set; }
        public string Nature { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime Created_at { get; set; }
        public int Created_By { get; set; }
        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }
}
