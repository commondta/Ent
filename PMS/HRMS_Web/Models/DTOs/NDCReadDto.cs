namespace HRMS_Web.Models.DTOs
{
    public class NDCReadDto
    {
        public string? NDCRequestType { get; set; }
        public string? TransferType { get; set; }
        public DateTime? SlotDate { get; set; }
        public DateTime? ValidateDate { get; set; }
        public string? SlotHour { get; set; }
        public string? SlotMintues { get; set; }
        public string? DealerCode { get; set; }
        public string? DealerName { get; set; }
        public string? EstateName { get; set; }
        public string? Day { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ApplyStation { get; set; }
    }
}
