namespace HRMS_Web.Models.DTOs
{
    public class COPDto
    {
        public int StockIdA { get; set; }
        public int StockIdB { get; set; }
        public string? Remarks { get; set; }
        public string? CurrentPropertyMarketValue { get; set; }
        public string? ProposedPropertyMarketValue { get; set; }
        public DateTime? COPDate { get; set; }
    }
}
