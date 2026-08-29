using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FormAlert : BaseModel
    {
        public int FormId { get; set; }

        public virtual ICollection<FormAlertUsers>? FormAlertUsers { get; set; }
    }

    public class FormAlertUsers : BaseModel
    {
        [ForeignKey("FormAlertId")]
        public int FormAlertId { get; set; }
        public FormAlert? FormAlert { get; set; }

        //UserId will be here Foreign Key after user module added
        public int UserId { get; set; }

        public string? UserDesignation { get; set; }
    }
}
