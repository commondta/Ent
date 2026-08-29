namespace HRMS_Web.Models.DTOs
{
    public class GetReuprchaseReadModel
    {
        public int Id { get; set; }
        public int? StockCreationId { get; set; }
        public string RegistrationNo { get; set; }
        public string PropertyNo { get; set; }
        public int MemberProfileId { get; set; }
        public string MemberName { get; set; }
        public string ConstracutionStatus { get; set; }
        public bool? PossessionStatus { get; set; }
        public string? ActualSize { get; set; }
        public string? Remarks { get; set; }
        public string? NetProfitLoss { get; set; }
        public string? PurchaseRefundValue { get; set; }
        public string? MarketValue { get; set; }
        public string? Type { get; set; }
        public string? CreditMemoType { get; set; }
        public string? BookingDate { get; set; }
        public DateTime DocDate { get; set; }
        public string? TotalRecieved { get; set; }
        public string? DeductionAmount { get; set; }
        public string? Balance { get; set; }
        public string? Dealer { get; set; }
        public string? AmountType { get; set; }
        public string? BlockName { get; set; }
        public string? CategoryName { get; set; }
        public List<RePurchasePropertyDivisionReadModel> RePurchasePropertyDivisions { get; set; }
        public List<MarketValueAssesmentReadModel> MarketValueAssesment { get; set; }
    }

    public class RePurchasePropertyDivisionReadModel
    {
        public int Id { get; set; }
        public string? RegPrefix { get; set; }
        public int? RegNumber { get; set; }
        public string? RegPostfix { get; set; }
        public string? Size { get; set; }
        public string? PropPrefix { get; set; }
        public int? PropNumber { get; set; }
        public string? PropPostfix { get; set; }
        public string? Category { get; set; }
    }

    public class MarketValueAssesmentReadModel
    {
        public int? DealerId { get; set; }
        public string? Remarks { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? CNIC { get; set; }
        public decimal? Value { get; set; }
    }

    public class Charges
    {
        public string ChargeName { get; set; }
        public int? Amount { get; set; }
    }
}
