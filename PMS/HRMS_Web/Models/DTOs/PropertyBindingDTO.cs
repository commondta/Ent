namespace HRMS_Web.Models.DTOs
{
    public class PropertyBindingDTO
    {
        public int Id { get; set; }
        public string? propertyNo { get; set; }
        public string? blockNo { get; set; }
        public string? category { get; set; }
        public string? registrationNo { get; set; }
    }
    public class RegistrationNoDto
    {
        public int Id { get; set; }
        public string? registrationNo { get; set; }
    }
}
