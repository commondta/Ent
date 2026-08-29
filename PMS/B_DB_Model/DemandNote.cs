using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class DemandNote : BaseModel
    {
        public string Status { get; set; } = String.Empty;
        public string RequesterName { get; set; } = String.Empty;
        public DateTime ValidUntill { get; set; }
        public DateTime RequiredDate { get; set; }
        public string MDNType { get; set; } = String.Empty;
        public string Remarks { get; set; } = String.Empty;
        public string Deparment { get; set; } = String.Empty;
        public string ItemGroupCode { get; set; } = String.Empty;
        public bool? IsDemandNoteRequested { get; set; }
        public bool? IsDemandNoteApproved { get; set; }
        public int? ManagerId { get; set; }
        public int? CustodianId { get; set; }
        public bool? ManagerAssigned { get; set; }
        public bool? CustodianAssigned { get; set; }
        public DateTime? ManagerApproved_At { get;set; }
        public DateTime? ManagerRejected_At { get;set; }
        public DateTime? CustodianApproved_At { get;set; }
        public DateTime? CustodianRejected_At { get;set; }
        public String? ManagerApprovedOrRejectRemarks { get;set; } = String.Empty;
        public String? CustodianApprovedOrRejectRemarks { get;set; } = String.Empty;
        public string DocEntry { get; set; } = String.Empty;
        
        public string DocNum { get; set; } = String.Empty;
        public bool SapPosting { get; set; }
        public bool? DNManagerStatus { get; set; }
        public bool? DNCustodianStatus { get; set; }
     

        //Navigation

        public virtual ICollection<DemandNoteItems>? DemandNoteItems { get; set; }
    }

    public class DemandNoteItems : BaseModel
    {
        public string ItemNo { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DateTime RequiredDate { get; set; }
        public decimal RequiredQuantity { get; set; }
        public decimal? LastPurcPrice { get; set; }
        public decimal? InfoPrice { get; set; }
        public string Whse { get; set; } = String.Empty;
        public string Uom { get; set; } = String.Empty;
        public string PrjCode { get; set; } = String.Empty;
        public string PrjName { get; set; } = String.Empty;
        public string Remarks { get; set; } = String.Empty;
        public bool isApproved { get; set; } = false;
        //Navigation
        [ForeignKey("DemandNoteId")]
        public int DemandNoteId { get; set; }
        public DemandNote? DemandNote { get; set; }

    }
}
