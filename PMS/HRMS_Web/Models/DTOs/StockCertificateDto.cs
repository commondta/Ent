using B_DB_Model;

namespace HRMS_Web.Models.DTOs
{
    public class StockCertificateDto
    {
        public int ID { get; set; }

        public string? PlotNo { get; set; }
        public string? FileNo { get; set; }
        public string? PropertyNo { get; set; }

        public string? RegistrationNo { get; set; }
        public string? CaseCode { get; set; }

        public string? SalePerson { get; set; }
        public string? ImageURL { get; set; }

        public string? RealStateTypeName { get; set; }
        public string? ProjectName { get; set; }
        public string? PhaseName { get; set; }
        public string? CategoryName { get; set; }
        public string? BlockName { get; set; }
        public string? NatureName { get; set; }
        public string? TypeName { get; set; }
        public string? PrefixProperty { get; set; }
        public string? ConstracutionStatus { get; set; }

        public string? ActualSize { get; set; }
        public string? Mouza { get; set; }
        public string? AllocationNo { get; set; }

        public string? SaleDeedNo { get; set; }
        public DateTime? SaleDeedDate { get; set; }

        public decimal MembershipFee { get; set; }
        public decimal MiscCharges { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? AllocationSignatoryDesignation { get; set; }
        public string? AllocationSignatoryName { get; set; }
        public string? AllocationSignatoryRank { get; set; }

        public DateTime? BookingDate { get; set; }
        public decimal? Amount { get; set; }

        public List<MemberName>? MemberNames { get; set; } = new();
    }
}
