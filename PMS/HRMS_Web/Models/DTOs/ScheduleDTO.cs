namespace HRMS_Web.Models.DTOs
{
    public class ScheduleDTO
    {
        public int StockCreationId { get; set; }
        public int MemberProfileId { get; set; }
        public string? Remarks { get; set; }
        public DateTime? PostingDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LastModifiedUserName { get; set; }

        public List<PaymentPlanDTO> PaymentPlan { get; set; }
    }

    public class PaymentPlanDTO
    {
        public string PaymentType { get; set; }
        public string ChargeTypeId { get; set; }
        public decimal Amount { get; set; }
        public decimal? Rebate { get; set; } = 0;
        public decimal NetAmount { get; set; }
        public int Days { get; set; }
        public DateTime? DueDate { get; set; }
        public string? PaymentFor { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
