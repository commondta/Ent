using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class BillingServiceTable : BaseModel
    {
        public int? MemberId { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? BillMonth { get; set; }
        public string? BillFor { get; set; }
        public bool? IsSapPosted { get; set; }
        public string? InvoiceNo { get; set; }
        [MaxLength(100)]
        public string? UniqueReferenceNo { get; set; }
        public string? ErrorMessage { get; set; }


        public virtual ICollection<BillingServiceDetailsTable>? BillingServiceDetailsTable { get; set; }

    }

    public class BillingServiceDetailsTable : BaseModel
    {
        public int? ChargeGroupTypeId { get; set; }
        public int? ChargeTypeId { get; set; }
        public string? SAPAccount { get; set; }
        public double? Rate { get; set; }
        public double? Quantity { get; set; }
        public string? MeterNo { get; set; }
        public decimal? NetAmount { get; set; }
        public string? CurrentReading { get; set; }
        public string? PreviousReading { get; set; }
        public string? Uom { get; set; }
        public DateTime? ReadingDate { get; set; }
        [MaxLength(100)]
        public string? UniqueReferenceNo { get; set; }
        public string? ReferenceNo { get; set; }
        public string? BillFor { get; set; }

        [ForeignKey("BillingServiceTableId")]
        public int? BillingServiceTableId { get; set; }
        public BillingServiceTable? BillingServiceTable { get; set; }
    }
}
