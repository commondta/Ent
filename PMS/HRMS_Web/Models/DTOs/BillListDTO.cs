namespace HRMS_Web.Models.DTOs
{
    public class BillListDTO
    {
        public string RegistrationNo { get; set; }
        public DateTime? DueDate { get; set; }
        public string BillMonth { get; set; }
        public decimal Bill { get; set; }
        public decimal Paid { get; set; }
        public decimal Outstanding { get; set; }
        public decimal Surcharge { get; set; }
        public string Status { get; set; }
    }
}
