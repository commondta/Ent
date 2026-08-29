using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class GlobalChargeSetupDetailFilterDTO
    {
        [Required]
        public int RealStateTypeId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int PhaseId { get; set; }
        [Required]
        public int BlockId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        [Required]
        public int NatureId { get; set; }
        public int Size { get; set; } = 0;
        public string Redesign { get; set; }
        public string? GeneratorUnitType { get; set; } = string.Empty;
    }

    public class GlobalChargeSetupDetailFixedChargFilterDTO
    {
        [Required]
        public int RealStateTypeId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int PhaseId { get; set; }
        [Required]
        public int BlockId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        [Required]
        public int NatureId { get; set; }
        public int FormId { get; set; }

        public string? ConstructionStatus { get; set; }
        public bool? PossessionStatus { get; set; }
        public bool? GracePeriod { get; set; }
    }

    public class GlobalChargeSetupNDCFilterDTO
    {
        [Required]
        public int RealStateTypeId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int PhaseId { get; set; }
        [Required]
        public int BlockId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        [Required]
        public int NatureId { get; set; }
    }

    public class GlobalChargesSellerGovtTaxFilterDTO
    {
        [Required]
        public string Filer { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        [Required]
        public int NatureId { get; set; }
        public int StockId { get; set; }
        public int PropertyTaxYears { get; set; }
        public string? ConstracutionStatus { get; set; }
        public bool FBRTAX236C { get; set; }
    }

    public class GlobalChargesBuyerGovtTaxFilterDTO
    {
        [Required]
        public string BFiler { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        [Required]
        public int NatureId { get; set; }
        public int StockId { get; set; }
        public string? ConstracutionStatus { get; set; }
        public bool RegistryVerification { get; set; }
    }

    public class GlobalChargeSetupNDCMemberFilterDTO
    {
        public string? ConstracutionStatus { get; set; }

        public string RequestType { get; set; }

        public string TransferType { get; set; }

        public int Category { get; set; }
    }

    public class GlobalChargeSetupWavieOffDTO
    {
        public string ConstracutionStatus { get; set; }

        public string Sector { get; set; }

        public int Block { get; set; }

        public int Category { get; set; }
    }

    public class GlobalChargeSetupNDCDealerFilterDTO
    {
        public string ConstracutionStatus { get; set; }

        public string RequestType { get; set; }

        public string TransferType { get; set; }
        public bool Processing { get; set; }

        [Required]
        public int Category { get; set; }
    }

    public class GlobalChargeSetupFileVerificationFilterDTO
    {
        public string ConstracutionStatus { get; set; }
    }

    public class GlobalChargeSetupFileRequestFilterDTO
    {
        public string RequestType { get; set; }
    }
}
