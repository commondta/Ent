using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class UserPermissionMapping : BaseModel
    {
        public int EMP_CODE { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }

        // Navigation

        [ForeignKey("PermissionFormsId")]
        public int? PermissionFormsId { get; set; }
        public PermissionForms? PermissionForms { get; set; }
    }
}
