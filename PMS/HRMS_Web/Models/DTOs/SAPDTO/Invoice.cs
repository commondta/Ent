using B_DB_Model;

namespace HRMS_Web.Models.DTOs.SAPDTO
{
    public class Invoice
    {
        public string DocNum { get; set; }
        public string DocEntry { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string Project { get; set; }
        public string U_PropertyNo { get; set; }
        public string Block { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string BalanceDue { get; set; }
        public string InvoiceCategory { get; set; }
       // public string Block { get; set; }
      
    }
    public class SingleInvoice
    {
        public string DocNum { get; set; }
        public string DocEntry { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string RegistrationNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string PropertyNum { get; set; }
        public string DocTotal { get; set; }
        public string TotalPaid { get; set; }
        public string BalanceDue { get; set; }
        public virtual ICollection<InvoiceDetail>? Details { get; set; }

    }
    public class InvoiceDetail
    {
        public string LineNum { get; set; }
        public string AccountCode { get; set; }
        public string ChargeName { get; set; }
        public string RegistrationNum { get; set; }
       
        public string LineTotal { get; set; }
    }
    public class SapCardNameAndCardCode
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }

    public class InvoiceCategory
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Count { get; set; }
    }

}
