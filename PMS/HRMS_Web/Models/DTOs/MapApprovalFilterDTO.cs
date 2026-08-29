namespace HRMS_Web.Models.DTOs
{
    public class DemarcationFormFilterDTO
    {
        public int ID { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public int? MemberCode { get; set; }
        public string? MemberName { get; set; }
        public string? CNIC { get; set; }
        public string? GracePeriodTime { get; set; }
        public string? Date { get; set; }
        public DateTime? ClientDemarmationDate { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
