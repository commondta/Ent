using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PermissionForms : BaseModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int SerialNo { get; set; }
    }
}
