namespace HRMS_Web.Models.DTOs
{
    public class BulkMeterBillGenerationDto
    {
        public decimal SumOfAmount { get; set; }
        public decimal SumOfConumedUnits { get; set; }

        public List<MeterBillGenerationDTO> MeterBillGenerationDTO { get; set;}
    }
    public class MeterBillGenerationDTO
    {
        public decimal? LastReading { get; set; }
        public decimal? CurrentReading { get; set; }
        public decimal? FuelAjustmentUnits { get; set; }
        public decimal? UnitsConsumed { get; set; }
        public decimal? PerUnitRate { get; set; }
        public decimal? SaleTax { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? SAPAccount { get; set; }
        public decimal WTax { get; set; }
        public string? MeterNo { get; set; }
        public decimal FuelAdjustment { get; set; }
        public decimal SaleTaxAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal WHTaxAmount  { get; set; }
    }

    public class MeterInstallationStockCreationDTO
    {
        public int ID { get; set; }
        public string RegistrationNo { get; set; }
        public string PropertyNo { get; set; }
        public string RealStateType { get; set; }
        public string Project { get; set; }
        public string Phase { get; set; }
        public string Block { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Nature { get; set; }
        public string GeneratorUnitType { get; set; }
    }

    public class ChargeResultDTO
    {
        public decimal PerUnitRate { get; set; }
        public string SapAccount { get; set; }
        
    }
}
