using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class ownershipAgreementDto
    {
        public string? SignatoryRank { get; set; } = String.Empty;
        public string? SignatoryDesignation { get; set; } = String.Empty;
        public string? SignatoryName { get; set; } = String.Empty;
        public string? TransferCertificateTimeLineStatement { get; set; } = String.Empty;
        public int? BuyerId { get; set; }
        public int? SellerId { get; set; }
        public string? SellerName { get; set; }
        public string? BuyerName { get; set; }
        public string? MembershipNo { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public string? RelationshipSeller { get; set; }
        public string? RelationshipBuyer { get; set; }
        public string? RelationshipDealer { get; set; }
        public string? RelationshipWithSeller { get; set; }
        public string? RelationshipWithBuyer { get; set; }
        public string? RelationshipWithDealer { get; set; }
        public string? BuyerDetail { get; set; }
        public string? BuyerCnic { get; set; }
        public string? BuyerPhone { get; set; }
        public string? DealerCnic { get; set; }
        public string? SellerCnic { get; set; }
        public string? PermanentAddress { get; set; }
        public string? SectorName { get; set; }
        public string? DealerRegistrationNo { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? Area { get; set; }
        public string? UnitArea { get; set; }
        public string? Sqft { get; set; }
        public string? Type { get; set; }
        public string? Block { get; set; }
        public string? Nature { get; set; }
        public bool? IsLetterPrint { get; set; }
        public string? Phase { get; set; }
        public string? Category { get; set; }
        public string? ConstructionStatus { get; set; }
        public int? TransferReciptId { get; set; }
        public string? TransferType { get; set; }
        public string? ApplyStation { get; set; }
        public string? SlotDate { get; set; }
        public DateTime? OpenSlotDate { get; set; }
        public DateTime? docDate { get; set; }
        public string? transferDate { get; set; }
        public string? BuyerRepresentativeName { get; set; }
        public string? BuyerRepresentativeRelationshipWith { get; set; }
        public string? BuyerRepresentativeCnic { get; set; }
        public string? Title { get; set; }
        public string? PlotNo { get; set; }
        public DateTime? LetterDate { get; set; } = DateTime.Now;
        public string? LegalHeireType { get; set; }
        public string? LagalHeireContent { get; set; }
        public string? SellerJointMembers { get; set; }
        public int? StockId { get; set; }

        // List of image URLs or paths
        public List<Url>? Images { get; set; }
        public List<MemberName>? MemberNames { get; set; }
        public string? BuyerMemberNames { get; set; }
        public string? SellerMemberNames { get; set; }
        public string? SwapOverStatement { get; set; }
        public string? Statement { get; set; }
    }

    public class OwnershipAgreementPrintDto
    {
        public string? SignatoryRank { get; set; } = string.Empty;
        public string? SignatoryDesignation { get; set; } = string.Empty;
        public string? SignatoryName { get; set; } = string.Empty;

        public string? DealerName { get; set; }
        public string? EstateName { get; set; }
        public string? DealerCnic { get; set; }

        public string? RelationshipBuyer { get; set; }
        public string? RelationshipWithBuyer { get; set; }

        public string? BuyerName { get; set; }
        public string? BuyerCnic { get; set; }
        public string? MembershipNo { get; set; }
        public string? BuyerPhone { get; set; }

        public string? DealerRegistrationNo { get; set; }
        public string? PermanentAddress { get; set; }

        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }

        public string? ConstructionStatus { get; set; }

        public string? Area { get; set; }
        public string? UnitArea { get; set; }
        public string? Sqft { get; set; }

        public string? RealEstateType { get; set; }
        public string? Type { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Nature { get; set; }
        public string? SectorName { get; set; }
        public string? Phase { get; set; }

        public string? Title { get; set; }
        public string? PlotNo { get; set; }
        public string? ImageUrl { get; set; }
        public string? BuyerWithJointMembers { get; set; }
        public string? SellerWithJointMembers { get; set; }
        public string? AmalgamatedPlot1 { get; set; }
        public string? AmalgamatedPlot2 { get; set; }
        public string? AmalgamatedReg1 { get; set; }
        public string? AmalgamatedReg2 { get; set; }
        public string? AmalgamatedSector { get; set; }
        public string? AmalgamatedPhase { get; set; }

        public List<MemberName>? MemberNames { get; set; }
    }


    public class Url
    {
        public string imageUrl { get; set; }
    }
    
    public class MemberName
    {
        public string? MemeberName { get; set; } 
        public string? Relationhipwith { get; set; } 
        public string? RelationName { get; set; } 
        public string? Cnic { get; set; } 
        public int? Id { get; set; } 
    }

    public class ClientFileDto
    {
        public int DocNum { get; set; }
        public int? stockId { get; set; }
        public string? MemberName { get; set; }
        public string? Relationship { get; set; }
        public string? RelationshipWith { get; set; }
        public string? Mobile { get; set; }
        public string? Cnic { get; set; }
        public string? ReceivedBy { get; set; }
        public string? RecieverFatherName { get; set; }
        public string? RecieverCNIC { get; set; }
        public string? RecieverMobile { get; set; }
        public string? PermanentAddress { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? Area { get; set; }
        public string? UnitArea { get; set; }
        public string? Sqft { get; set; }
        public string? Type { get; set; }
        public string? Block { get; set; }
        public string? Nature { get; set; }
        public string? Floor { get; set; }
        public DateTime? docDate { get; set; }

        public string Image { get; set; }

        public List<JointMemberDto> JointMembers { get; set; } = new List<JointMemberDto>();
    }

   
}
