using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Inovice : BaseModel
    {
        public int? ChargeTypeId { get; set; }
        public string? ChargeType { get; set; }
        public string? FormName { get; set; }
        public string? RegistrationNo { get; set; }
        public int? FormId { get; set; }
        public string? SapAccount { get; set; }
        public short InvoiceGroup { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime? DueDate { get; set; }
        public double? Amount { get; set; }
        public double? AmountDue { get; set; }
        public double? AmountRecieved { get; set; }
        public double? DocTotal { get; set; }
        public string? InvoiceNo { get; set; }
        public string? ReciptNo { get; set; }
        public bool? IsInProress { get; set; }
        public bool? IsSapPosting { get; set; }
        public bool? IsCancelled { get; set; }
        public string? Remarks { get; set; }
        public string? ErrorMessage { get; set; }

        public string? InstallementNo { get; set; }
        public int? Days { get; set; }
        public int? PaymentTypeId { get; set; }
        public string? ReferenceNo { get; set; }

        //Navigation
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }
    }
}
