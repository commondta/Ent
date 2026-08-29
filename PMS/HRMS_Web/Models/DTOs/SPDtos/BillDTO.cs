namespace HRMS_Web.Models.DTOs.SPDtos
{
    public class BillDTO
    {
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MeterNo { get; set; }
        public string? Arrears { get; set; }
        public double NetAmount { get; set; }
    }
}
