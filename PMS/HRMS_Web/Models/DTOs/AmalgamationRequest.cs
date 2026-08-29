namespace HRMS_Web.Models.DTOs
{
    public class AmalgamationRequest
    {
        public List<int> PropertyIds { get; set; }

        public string? NewRegistrationNo { get; set; }

        public string? Prefix { get; set; }
        public string? Postfix { get; set; }
        public int? Number { get; set; }

        public int? PhaseId { get; set; }
        public int? RealEstateTypeId { get; set; }
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }

        public string? Remarks { get; set; }
        public string? UserName { get; set; }
    }
}
