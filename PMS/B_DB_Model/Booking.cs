using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{

    public class Booking : BaseModel
    {
        public string? ImageURL { get; set; }
        public bool JointOwners { get; set; }
        public string? Remarks { get; set; }
        [DataType("decimal(18,2)")]
        public decimal BookingPrice { get; set; }
        [DataType("decimal(18,2)")]
        public decimal AdvanceAmount { get; set; }
        [DataType("decimal(18,2)")]
        public decimal BalanceAmount { get; set; }
        public string? AdvanceReceiptNo { get; set; } = string.Empty;
        public string? AdvanceReceiptDate { get; set; } = string.Empty;
        public string? BookingConfirmationReceiptNo { get; set; } = string.Empty;   
        public string? BookingConfirmationReceiptDate { get; set; } = string.Empty;
        public string? CorrespondenceAddress { get; set; }
        public string? CorrespondenceEmail { get; set; }
        public string? CorrespondenceMobileNo { get; set; }
        public string? CorrespondenceWhatsappNo { get; set; }
        public bool? isBookingProcessingChargesPostedInSap { get; set; }
        public string? BookingProcessingChargesErrorMsg { get; set; } = string.Empty;
        public bool? isBookingPaymentSchedulePostedInSap { get; set; }
        public string? BookingPaymentScheduleErrorMsg { get; set; } = string.Empty;

        [NotMapped]
        public string? SalePerson { get; set; } = string.Empty;

        [NotMapped]
        public string? AllocationSignatoryRank { get; set; } = String.Empty;
        [NotMapped]
        public string? AllocationSignatoryDesignation { get; set; } = String.Empty;
        [NotMapped]
        public string? AllocationSignatoryName { get; set; } = String.Empty;


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

        public virtual ICollection<BookingProcessingCharges>? BookingProcessingCharges { get; set; }
        public virtual ICollection<BookingSchedulePaymentPlanDetail>? BookingSchedulePaymentPlanDetail { get; set; }
        public virtual ICollection<BookingJointMember>? BookingJointMember { get; set; }
        public virtual ICollection<BookingNominee>? BookingNominee { get; set; }

    }

    public class BookingProcessingCharges : BaseModel
    {
        public string? FeeType { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public int? ChargeTypeId { get; set; }
        public string? Remarks { get; set; }
        public string? SapAccount { get; set; }

        [ForeignKey("BookingId")]
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }

    public class BookingSchedulePaymentPlanDetail : BaseModel
    {
        public string? InstallementNo { get; set; }
        public int? ChargeTypeId { get; set; }
        public int Days { get; set; }
        public DateTime? DueDate { get; set; }

        [DataType("decimal(18,2)")]
        public decimal? Amount { get; set; }
        public string? ReciptNo { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public int? PaymentTypeId { get; set; }
        [ForeignKey("BookingId")]
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }

    public class BookingJointMember : BaseModel
    {

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
        public string? Remarks { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? CNIC { get; set; }

        [ForeignKey("BookingId")]
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }

    public class BookingNominee : BaseModel
    {

        public string Name { get; set; }
        public string Relationship { get; set; }
        public string CNIC { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }

        [ForeignKey("BookingId")]
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
