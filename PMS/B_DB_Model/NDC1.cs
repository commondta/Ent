using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class NDC1 : BaseModel
    {
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        public bool? IsNDC1Requested { get; set; }
        public bool? IsNDC1Approved { get; set; }
        public string? DealerCode { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public string? NDCRequestType { get; set; }
        public string? TransferType { get; set; }
        public string? Outstation { get; set; }
        public string? SlotDate { get; set; }
        public string? Slot { get; set; }
        public string? ValidityDate { get; set; }
        public string? Day { get; set; }
        [MaxLength(100)]
        public string? TransferPurpose { get; set; }
        public string? Remarks { get; set; }
        public bool? IsCanceled { get; set; }
        public bool IsRequestClosed{ get; set; }
        public bool? IsGovtTaxRequested { get; set; }
        public bool? IsGovtTaxApproved { get; set; }
        [MaxLength(100)]
        public string? ApplyStation { get; set; }

        public virtual ICollection<NDC1PowerOfAttorey>? NDC1PowerOfAttorey { get; set; }
        public virtual ICollection<NDC1CheckList>? NDC1CheckList { get; set; }
        public virtual ICollection<NDC1Attachments>? NDC1Attachments { get; set; }
    }

    public class NDC1PowerOfAttorey : BaseModel
    {
        public string? Name { get; set; }
        public string? Cnic { get; set; }

        [ForeignKey("NDC1Id")]
        public int? NDC1Id { get; set; }
        public NDC1? NDC1 { get; set; }
    }

    public class NDC1CheckList : BaseModel
    {
        public string? Department { get; set; }
        public string? Remarks { get; set; }
        public string? AlertNarration { get; set; }
        public string? Date { get; set; }

        [ForeignKey("NDC1Id")]
        public int? NDC1Id { get; set; }
        public NDC1? NDC1 { get; set; }
    }
    public class NDC1Attachments : BaseModel
    {
        public string DoucmentName { get; set; }
        public string Document { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("NDC1Id")]
        public int? NDC1Id { get; set; }
        public NDC1? NDC1 { get; set; }
    }
}
