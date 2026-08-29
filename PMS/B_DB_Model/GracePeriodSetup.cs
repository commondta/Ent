using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class GracePeriodSetup : BaseModel
    {
        public int? PossessionGracePriod{ get; set; }
        public int? TransferGracePeriod{ get; set; }
    }
}
