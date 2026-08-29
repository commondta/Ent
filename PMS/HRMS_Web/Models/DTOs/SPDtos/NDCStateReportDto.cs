namespace HRMS_Web.Models.DTOs.SPDtos
{
    public class NDCStateReportDto
    {
        public int Id { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MemberName { get; set; }
        public string? RelationshipWith { get; set; }
        public string? Cnic { get; set; }
        public string? MembershipNo { get; set; }
        public string? SellerName { get; set; }
        public string? BuyerName { get; set; }
        public string? TransferCharges { get; set; }
        public string? Phase { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public string? TransferReceiptTaxPayer { get; set; }
        public string? Status { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? ValidityDate { get; set; }

        public string? FormattedDateTime
        {
            get
            {
                return DateTime?.ToString("dd-MM-yyyy");
            }
        }

        public string? formattedValidityDate
        {
            get
            {
                return ValidityDate?.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }

        public string? Sector { get; set; }
        public string? DealerName { get; set; }
        public string? EstateName { get; set; }
        public string? ApplyStation { get; set; }
        public string? Depositor { get; set; }
        public string? TransferType { get; set; }
        public string? NDCRequestType { get; set; }
    }

    public class PropertyInfoDTO
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string? FormattedDateTime
        {
            get
            {
                return DateTime.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }
        public string? PropertyNo { get; set; }
        public string? RegistrationNo { get; set; }
        public string? Sector { get; set; }
        public string? PropertyStatus { get; set; }
        public string? Almt { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public string? MemberName { get; set; }
        public string? RelationshipWith { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? CurrentAddress { get; set; }
        public string? MembershipNo { get; set; }
        public string? Cnic { get; set; }
        public string? Phase { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Quota { get; set; }
        public string? Status { get; set; }
    }

    public class KeyValuePairDto
    {
        public string? Name { get; set; }
        public string? Count { get; set; }
    }

}
