using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FormsModel 
    {
        public string FormName { get; set; }

        public int FormId { get; set; }
    }
    public class FormsChargeGroup : BaseModel
    {
        public int FormId { get; set; }
        public int ChargeGroupId{ get; set; }
    }

    public class FormsChargeGroupRequestDto
    {
        public int FormId { get; set; }
        public List<int> ChargeGroupIds { get; set; }
    }

}
