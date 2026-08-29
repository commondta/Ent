namespace HRMS_Web.Models.DTOs.SPDtos
{
    public class TransferSetReceivingDto
    {
        public int Id { get; set; }
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
        public DateTime? DateTime { get; set; }

        public string? FormattedDateTime
        {
            get
            {
                return DateTime?.ToString("dd-MM-yyyy, hh:mmtt");
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

    public class RecordRoomFileInOutReportDto
    {
        public int Id { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? PageOutIn { get; set; }
        public string? Remarks { get; set; }
        public string? Phase { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public DateTime? DateTime { get; set; }

        public string? FormattedDateTime
        {
            get
            {
                return DateTime?.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }

        public DateTime? LastModifiedDateTime { get; set; }

        public string? LastModifiedDateTimeFormattedDateTime
        {
            get
            {
                return LastModifiedDateTime?.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }

        public DateTime? ExpectedReceivingDate { get; set; }

        public string? ExpectedReceivingDateFormattedDateTime
        {
            get
            {
                return ExpectedReceivingDate?.ToString("dd-MM-yyyy");
            }
        }

        public string? Sector { get; set; }

    }

    public class CautionReportDto
    {
        public int Id { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? SoftLockName { get; set; }
        public string? MembershipNo { get; set; }
        public string? MemberName { get; set; }
        public string? Phase { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public DateTime? DateTime { get; set; }

        public string? FormattedDateTime
        {
            get
            {
                return DateTime?.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }

        public DateTime? StartDate { get; set; }

        public string? StartDateFormattedDateTime
        {
            get
            {
                return StartDate?.ToString("dd-MM-yyyy");
            }
        }

        public DateTime? EndDate { get; set; }

        public string? EndDateFormattedDateTime
        {
            get
            {
                return EndDate?.ToString("dd-MM-yyyy");
            }
        }

        public string? Sector { get; set; }

    }

    public class TaxReportDto
    {
        public int Id { get; set; }
        public DateTime? DateTime { get; set; }

        public string? FormattedDateTime
        {
            get
            {
                return DateTime?.ToString("dd-MM-yyyy, hh:mmtt");
            }
        }
        public string? NDCRequestType { get; set; }
        public string? TransferType { get; set; }
        public string? ApplyStation { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public string? RegistrationNo { get; set; }
        public string? Sector { get; set; }
        public string? MemberName { get; set; }
        public string? RelationshipWith { get; set; }
        public string? MembershipNo { get; set; }
        public string? Cnic { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerRelationshipWith { get; set; }
        public string? BuyerMembershipNo { get; set; }
        public string? BuyerCnic { get; set; }
        public string? Phase { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? PropertyType { get; set; }
        public string? SellerDates { get; set; }
        public string? SellerAmounts { get; set; }
        public string? SellerTaxDescriptions { get; set; }
        public string? SellerChallanNos { get; set; }
        public string? BuyerDates { get; set; }
        public string? BuyerAmounts { get; set; }
        public string? BuyerTaxDescriptions { get; set; }
        public string? BuyerChallanNos { get; set; }
        public string? PropertyNo { get; set; }
    }

}
