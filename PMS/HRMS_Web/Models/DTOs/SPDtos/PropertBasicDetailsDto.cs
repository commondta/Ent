namespace HRMS_Web.Models.DTOs.SPDtos
{
    public class PropertBasicDetailsDto
    {
        public int Id { get; set; }
        public int? MemberNdcId { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MemberName { get; set; }
        public string? RelationshipWith { get; set; }
        public string? Cnic { get; set; }
        public string? MembershipNo { get; set; }
        public string? Phase { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public string? TransferReceiptTaxPayer { get; set; }
        public string? Status { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime? DateTime { get; set; }

        public string? FormattedDateTime
        {
            get
            {
                return DateTime?.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }
    }

    public class BasicPropertBasicDetailsDto
    {
        public int? ID { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? ActualSize { get; set; }
        public string? ActualSizeUnit { get; set; }
        public string? CoveredArea { get; set; }
        public string? ConstracutionStatus { get; set; }
        public string? PossessionStatus { get; set; }
        public string? MemberName { get; set; }
        public string? Cnic { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ImageURL { get; set; }
        public string? CnicFrontURL { get; set; }
        public string? CnicBackURL { get; set; }
        public string? Phase { get; set; }
        public string? Project { get; set; }
        public string? RealEstate { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public string? Nature { get; set; }
        public string? Floor { get; set; }
        public string? Feature { get; set; }
        public string? PropertyStatus { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }

    public class MemberBasicDetailsDto
    {
        public int Id { get; set; }
        public string MembershipNo { get; set; }
        public string MemberName { get; set; }
        public string RelationshipWith { get; set; }
        public string Cnic { get; set; }
        public string Mobile { get; set; }
        public string DateTime { get; set; }
    }
}
