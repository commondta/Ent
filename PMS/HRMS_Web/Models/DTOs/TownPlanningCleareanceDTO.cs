namespace HRMS_Web.Models.DTOs
{
    public class TownPlanningCleareanceDTO
    {
        public string? ChargeID { get; set; }
        public string? ChargeType { get; set; }

        public int DocEntry { get; set; }
        public int LineNum { get; set; }

        public string? AcctCode { get; set; }
        public int? BranchId { get; set; } = 1;

        public string? Project { get; set; }
        public string? AcctName { get; set; }

        public int DocNum { get; set; }

        public string? AccntntCod { get; set; }

        public decimal DocTotal { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidToDate { get; set; }

        public string? ReceiptNum { get; set; }
    }

}
