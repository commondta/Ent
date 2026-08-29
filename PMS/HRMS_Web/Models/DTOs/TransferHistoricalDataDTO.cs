namespace HRMS_Web.Models.DTOs
{
    public class TransferHistoricalDataDTO
    {
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public DateTime? TransferDate { get; set; }
        public string? SellerName { get; set; }
        public string? SellerCNIC { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerCNIC { get; set; }
        public string? Source { get; set; }
    }
}
