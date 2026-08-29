using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PropertyNo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Prefix { get; set; }
        public string Postfix { get; set; }
        public string Number { get; set; }
        public int Quantity { get; set; }
        public DateTime Created_at { get; set; }
       
        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }
        public bool? is_active { get; set; }
        public bool? is_deleted { get; set; }
    }
}
