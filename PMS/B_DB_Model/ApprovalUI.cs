using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ApprovalUI : BaseModel
    {
        [Required]
        [MaxLength(200)]
        public string ModuleORSubModule { get; set; }

        [Required]
        public int SerialNo { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public bool Checked { get; set; }

    }
}
