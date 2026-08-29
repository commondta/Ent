namespace HRMS_Web.Models.DTOs.SAPDTO
{
    public class InvoicePostingDTO
    {
        public DateTime DueDate { get; set; }
        public int StockId { get; set; }
        public int ChargeSetUpId { get; set; }
        public ICollection<InvoicePostingDetail> Details {get;set;}
    }
    public  class InvoicePostingDetail
    {
        public int ChargeID { get; set; }
        public decimal Amount { get; set; }
    }
}
