using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class IndividualBill : BaseModel
    {
        public string Month { get; set; } = string.Empty;
        public string BillFor { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public bool? IsIndividualBillRequested { get; set; }
        public bool? IsIndividualBillApproved { get; set; }
        
        //Navigation
        [ForeignKey("StockCreationID")]
        public int? StockCreationID { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        public virtual ICollection<IndividualBillDetail>? IndividualBillDetail { get; set; }

    }

    public class IndividualBillDetail : BaseModel
    {
        public string BillType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Surcharge { get; set; }
        public string OtherDuesDescription { get; set; } = string.Empty;
        public int OtherDuesAmount { get; set; }
        public int GrossAmount { get; set; }
        public int Discount { get; set; }
        public int NetAmount { get; set; }

        //Navigation
        [ForeignKey("IndividualBillId")]
        public int? IndividualBillId { get; set; }
        public virtual IndividualBill? IndividualBill { get; set; }
    }
}
