using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class CaseProfile : BaseModel
    {
        public string CaseId { get; set; } = String.Empty;
        public string CaseTitle { get; set; } = String.Empty;
        public string CaseFor { get; set; } = String.Empty;
        public string LandArea { get; set; } = String.Empty;
        public string FIRReferenceNo { get; set; } = String.Empty;
        public int AdvanceDeposit { get; set; }
        public string SettlementMark { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public string ReferenceOfSettlement { get; set; } = String.Empty;
        public string Reason { get; set; } = String.Empty;

        public string TermsAndConditionsOfLawyer { get; set; } = String.Empty;
        public int LawyerFee { get; set; }
        public int CourtFee { get; set; }
        public string Outcome { get; set; } = String.Empty;

        public bool? IsCaseProfileRequested { get; set; }
        public bool? IsCaseProfileApproved { get; set; }

        //Navigation
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        [ForeignKey("CaseTypeId")]
        public int? CaseTypeId { get; set; }
        public virtual CaseType? CaseType { get; set; }

        [ForeignKey("CaseCategoryId")]
        public int? CaseCategoryId { get; set; }
        public virtual CaseCategory? CaseCategory { get; set; }

        [ForeignKey("ForumId")]
        public int? ForumId { get; set; }
        public virtual Forum? Forum { get; set; }

        [ForeignKey("LawyerDataId")]
        public int? LawyerDataId { get; set; }
        public virtual LawyerData? LawyerData { get; set; }

        public virtual ICollection<CaseProfileParties>? CaseProfileParties { get; set; }
        public virtual ICollection<CaseProfileCaseHearings>? CaseProfileCaseHearings { get; set; }
        public virtual ICollection<CaseProfileNotices>? CaseProfileNotices { get; set; }
        public virtual ICollection<CaseProfileAppeals>? CaseProfileAppeals { get; set; }
        public virtual ICollection<CaseProfileAttachments>? CaseProfileAttachments { get; set; }
    }

    public class CaseProfileParties : BaseModel
    {
        public string Type { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string CNIC { get; set; } = String.Empty;
        public string MobileNo { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        //Navigation
        [ForeignKey("CaseProfileId")]
        public int? CaseProfileId { get; set; }
        public virtual CaseProfile? CaseProfile { get; set; }
    }

    public class CaseProfileCaseHearings : BaseModel
    {
        public DateTime Date { get; set; }
        public string Time { get; set; } = String.Empty;
        public string Proceeding { get; set; } = String.Empty;
        public string Remind { get; set; } = String.Empty;

        //Navigation
        [ForeignKey("CaseProfileId")]
        public int? CaseProfileId { get; set; }
        public virtual CaseProfile? CaseProfile { get; set; }
    }

    public class CaseProfileNotices : BaseModel
    {
        public string From { get; set; } = String.Empty;
        public DateTime Date { get; set; }
        public string Detail { get; set; } = String.Empty;
        public string Response { get; set; } = String.Empty;

        //Navigation
        [ForeignKey("CaseProfileId")]
        public int? CaseProfileId { get; set; }
        public virtual CaseProfile? CaseProfile { get; set; }
    }

    public class CaseProfileAppeals : BaseModel
    {
        public DateTime Date { get; set; }
        public string Time { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        //Navigation
        [ForeignKey("CaseProfileId")]
        public int? CaseProfileId { get; set; }
        public virtual CaseProfile? CaseProfile { get; set; }
    }

    public class CaseProfileAttachments : BaseModel
    {
        public string AttachmentPath { get; set; } = String.Empty;
        public string AttachmentName { get; set; } = String.Empty;
        public DateTime AttachmentDate  { get; set; } 
        public string Remarks  { get; set; } = String.Empty;

        //Navigation
        [ForeignKey("CaseProfileId")]
        public int? CaseProfileId { get; set; }
        public virtual CaseProfile? CaseProfile { get; set; }
    }
}
