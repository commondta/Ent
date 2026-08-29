namespace HRMS_Web.Models.DTOs.SAPDTO
{
    public class MDN
    {
    }
    public class Departs
    {
        public string Name { get; set; }
        public string CustodianId { get; set; }
        public int Id { get; set; }
    }
    public class Projects
    {
        public string PrjCode { get; set; }
        public string PrjName { get; set; }
    }
    public class ItemMaster
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string UomCode { get; set; }
        public string LastPurPrc { get; set; }
        public string DfltWH { get; set; }

    }
    public class WareHouse
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }


    }
    public class UOM
    {
        public string UomCode { get; set; }
        public string UomName { get; set; }


    }

    public class SurchargeSetupDTO
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal SurchargeAmount { get; set; }

    }


    public class NewClearanceDTO
    {
        public string InvoiceNo { get; set; }
        public DateTime? DueDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceOwed { get; set; }
        public string SettleDocNum { get; set; }
        public DateTime? SettleDate { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal Adjustment { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal Surcharge { get; set; }
        public decimal WaiveOffApplied { get; set; }
        public decimal TotalAmountReceivable { get; set; }
        public decimal FinalBalance { get; set; }
        public decimal SurchargeReceivedApplied { get; set; }
        public string Remarks { get; set; }
    }


    public class CleareanceDTO
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string AcctCode { get; set; }
        public string Project { get; set; }
        public string Remarks { get; set; }
        public DateTime DocDueDate { get; set; }
        public string AcctName { get; set; }
        public string InvCat { get; set; }
        public int DocNum { get; set; }
        public string AccntntCod { get; set; }
        public double DocTotal { get; set; }
        public double TotalRecieved { get; set; }
        public double BalanceDue { get; set; }
        public double PaidToDate { get; set; }
        public string ReceiptNum { get; set; }
        public string ReceiptDate { get; set; }


    }
    public class InvoiceRecordDTO
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public string CardName { get; set; }
        public double DocTotal { get; set; }

    }
    public class RegistrationAgainstInvoice
    {

        public string Registration { get; set; }
        public string Property { get; set; }
        public string Member { get; set; }


    }
    public class SingleInvoiceDTO
    {
        public int DocEntry { get; set; }
        public ICollection<SingleInvoiceChargeDTO> details { get; set; }


    }
    public class SingleInvoiceChargeDTO
    {
        public int LineNum { get; set; }
        public string AcctCode { get; set; }
        public string AcctName { get; set; }
        public double CreditMemo { get; set; }
        public double Price { get; set; }
        public double Adjustment { get; set; }


    }

    public class InvoiceSurchargeReportDto
    {
        public int DocEntry { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTime? DueDate { get; set; }

        public string? AccountCode { get; set; }

        public string? AccountName { get; set; }

        public string? Description { get; set; }

        public decimal? InvoiceAmount { get; set; }

        public decimal BalanceOwed { get; set; }

        public int? SettleDocNum { get; set; }

        public DateTime? SettleDate { get; set; }

        public decimal ReceivedAmount { get; set; }

        public decimal Adjustment { get; set; }

        public decimal BalanceDue { get; set; }

        public string? PaymentMethod { get; set; }

        public decimal Surcharge { get; set; }

        public decimal WaiverPaid { get; set; }

        public decimal SurchargePaymentPaid { get; set; }

        public decimal TotalAmountDue { get; set; }
    }

    public class ClearanceSummaryDto
    {
        public string AccountCode { get; set; }
        public string ChargeType { get; set; }

        public decimal BalanceOwed { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal BalanceDue { get; set; }

        public decimal Surcharge { get; set; }
        public decimal WaiverOff { get; set; }
        public decimal SurchargeReceivedApplied { get; set; }

        public decimal TotalAmountDue { get; set; }
    }


}
