using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class RolesPermissions : BaseModel
    {
        public string Designation { get; set; }

        public ICollection<Permissions> Permissions { get; set; }
    }

    public class Permissions : BaseModel
    {
        public string FormName { get; set; }

        public int RolesPermissionsId { get; set; }

        public bool isPermitted { get; set; }
    }
}
