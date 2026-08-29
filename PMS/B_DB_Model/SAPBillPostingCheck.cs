using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SAPBillPostingCheck : BaseModel
    {
        public int MemberId { get; set; }
        public string RegistrationNo { get; set; }
        public string Month { get; set; }
        public string BillMonth { get; set; }
        public string BillFor { get; set; }
        public string Comment { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DocDate { get; set; }
        public bool PostSAP { get; set; }
    }
}
