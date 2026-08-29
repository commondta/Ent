namespace HRMS_Web.Models.DTOs.SPDtos
{
    public class NDCReceiptDto
    {
        public int NdcNo { get; set; }
        public DateTime NdcDate { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PlotNo { get; set; }
        public string? MemberName { get; set; }
        public string? PlotSize { get; set; }
        public string? PlotType { get; set; }
        public string? SubmittedBy { get; set; }
        public string? EstateName { get; set; }
        public DateTime TransferDate { get; set; }
        public string? TransferTime { get; set; }
        public string? RequestType { get; set; }
        public string? TransferType { get; set; }
        public string? Block { get; set; }
        public string? ConstracutionStatus { get; set; }
        public string? PossessionStatus { get; set; }
    }
}
