using B_DB_Model;
using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class BillPrintDTO
    {
        public int? StockId { get; set; }
        public int? MemberId { get; set; }
        public int? ArInvoiceSeries { get; set; }
        public decimal? SaleTax { get; set; }
        public decimal? SaleTaxAmount { get; set; } = 0;
        public string? MemberName { get; set; }
        public string? Address { get; set; }
        public string? MobileNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? TenantMember { get; set; }
        public string? TenantMobileNo { get; set; }
        public string? Area { get; set; }
        public string? UOM { get; set; }

        public string? Size { get; set; }
        public string? ConsumerNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? RegistrationNo { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? DocDate { get; set; }
        public string? BillMonth { get; set; }
        public decimal? Arrears { get; set; }

        public string? MeterPicture { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string? FuelAdjustmentMonth { get; set; }

        public decimal? GrandFixedWTaxAmount { get; set; } = 0;
        public decimal? GrandMeterWTaxAmount { get; set; } = 0;
        public decimal? GrandMeterBillAmount { get; set; } = 0;
        public decimal? GrandFixedBillAmount { get; set; } = 0;
        public int Advance { get; set; } = 0;
        public int MaintenceAdvanceBillPaid { get; set; } = 0;

        [MaxLength(100)]
        public string? BillPrintRegistrationNo { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? BillPrintPropertyNo { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? BillPrintName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? BillPrintAddress { get; set; } = string.Empty;

        public decimal? BillBeforePRATax { get; set; } = 0;
        public decimal? CurrentBill { get; set; } = 0;
        public decimal? BillBeforeDueDate { get; set; } = 0;
        public decimal? SurchargeAfterDueDate { get; set; } = 0;
        public decimal? BillAfterDueDate { get; set; } = 0;
        public string? QrString { get; set; }
        public string? Remarks { get; set; }

        public List<MeterBillGenerationDetail>? MeterBillGenerationDetail { get; set; } = new List<MeterBillGenerationDetail>();
        public List<FixedChargeBillDetail>? FixedChargeBillDetail { get; set; } = new List<FixedChargeBillDetail>();
        public List<FixedChargeBillWHApplied>? FixedChargeBillWHApplied { get; set; } = new List<FixedChargeBillWHApplied>();
        public List<WTaxMapDTOPropertywise>? WTaxMapDTOPropertywise { get; set; } = new List<WTaxMapDTOPropertywise>();
        public List<MeterBillWithFixedCharge>? MeterBillWithFixedCharges { get; set; } = new List<MeterBillWithFixedCharge>();
        public List<PreviousBillDetailDTO>? PreviousBillDetailDTO { get; set; } = new List<PreviousBillDetailDTO>();
    }

    public class ReadingDetailDTO
    {
        public string? MeterPicture { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string? FuelAdjustmentMonth { get; set; }
    }

    public class PreviousBillDetailDTO
    {
        public string? Month { get; set; }
        public int? TotalAmount { get; set; }
        public int? PendingAmount { get; set; }
        public string? Status { get; set; }
    }

    public class DownloadBillDTO
    {
        public string? BillReferenceNo { get; set; }
        public string[]? DocEntry { get; set; }
        public string? MemberName { get; set; }
        public int? MemberId { get; set; }
        public string? Address { get; set; }

        public string? PropertyNo { get; set; }
        public string? RegistrationNo { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? DocDate { get; set; }

        public string? BillMonth { get; set; }

        public decimal? Arrears { get; set; }

        public decimal? CurrentBill { get; set; }

        public decimal? BillBeforeDueDate { get; set; }

        public decimal? BillAfterDueDate { get; set; }

        public decimal? Advance { get; set; }
        public decimal? Balance { get; set; }

        public string? Remarks { get; set; }

        public List<DownloadBillChargeDTO> Charges { get; set; } = new();

        public List<PreviousBillDetailDTO> PreviousBills { get; set; } = new();
    }

    public class DownloadBillChargeDTO
    {
        public string? ChargeType { get; set; }

        public decimal? Amount { get; set; }
    }
}
