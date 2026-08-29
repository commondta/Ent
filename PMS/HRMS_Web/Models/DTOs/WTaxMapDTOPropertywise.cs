namespace HRMS_Web.Models.DTOs
{
    public class WTaxMapDTOPropertywise
    {
        public string Month { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string? TaxCode { get; set; }
        public decimal NetAmount { get; set; }
        public decimal WHPercentage { get; set; }
        public decimal Amount { get; set; }
    }
    public class MeterBillWithFixedCharge
    {
        public string SAPAccount { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string MeterNo { get; set; } = string.Empty;
        public string SaleTax { get; set; } = string.Empty;
        public string CurrentReading { get; set; } = string.Empty;
        public string PreviousReading { get; set; } = string.Empty;
        public DateTime ReadingDate { get; set; }
        public string Uom { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
    }
}
