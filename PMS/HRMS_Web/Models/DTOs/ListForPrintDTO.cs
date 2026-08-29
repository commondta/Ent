namespace HRMS_Web.Models.DTOs
{
    public class ListForPrintDTO
    {
        public DateTime? DocDate { get; set; }
        public string? BillMonth { get; set; }
        public string? BillFor { get; set; }    
        public DateTime? DueDate { get; set; }
        public string? Remarks { get; set; }

        public List<BillList>? BillList { get; set; }
    }

    public class BillList
    {
        public string RegistrationNo { get; set; }
        public string PropertyNo { get; set; }
        public string MeterNo { get; set; }
        public decimal Amount { get; set; }
    }
}
