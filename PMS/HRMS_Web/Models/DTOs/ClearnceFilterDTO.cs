namespace HRMS_Web.Models.DTOs
{
    public class ClearnceFilterDTO
    {
        public int ID { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public int? MemberCode { get; set; }
        public string? MemberName { get; set; }
        public string? CNIC { get; set; }
        public string RedesignRequest { get; set; }
    }
}
