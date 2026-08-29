namespace HRMS_Web.Models.DTOs
{
    public class RenumberDto
    {
        public int? ID { get; set; }
        public string? CategoryName { get; set; }
        public string? BlockName { get; set; }
        public string? ActualSize { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MemberName { get; set; }
        public string? Cnic { get; set; }
    }

    public class SelectCOPDto
    {
        public int? ID { get; set; }
        public string? CategoryName { get; set; }
        public string? BlockName { get; set; }
        public string? Type { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MemberName { get; set; }
        public string? Cnic { get; set; }
    }

}
