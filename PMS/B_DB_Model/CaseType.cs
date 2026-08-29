using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class CaseType : BaseModel
    {
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = String.Empty;
    }
}
