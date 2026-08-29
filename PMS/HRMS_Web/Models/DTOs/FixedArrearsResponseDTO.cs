namespace HRMS_Web.Models.DTOs
{
    public class FixedArrearsResponseDTO
    {
        public string RegistrationNo { get; set; }
        public string BillMonth { get; set; }

        public string BillReferenceNo { get; set; }

        public decimal BillAmount { get; set; }

        public decimal Arrears { get; set; }

        public DateTime BillDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Remarks { get; set; }

        public List<string> DocEntries { get; set; } = new();

        public List<DownloadBillChargeDTO> Charges { get; set; } = new();

        public List<PreviousBillDetailDTO> PreviousBills { get; set; } = new();
    }
}
