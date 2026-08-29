using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MemberProfile : BaseModel
    {
        public string? ImageURL { get; set; } 
        public string? CNICFront { get; set; } 
        public string? CNICBack { get; set; } 
        public string? HonorificsName { get; set; } 
        public string? MemberName { get; set; } 
        public string? Relationship { get; set; } 
        public string? RelationshipWith { get; set; } 
        public string? MemberStatus { get; set; } 
        public string? MemberCategory { get; set; } 
        public string? Rank { get; set; } 
        public string? Force { get; set; } 
        public string? PANO { get; set; } 
        public string? Shaheed { get; set; } 
        public string? Quota { get; set; } 
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; } 
        public string? Cnic { get; set; } 
        public DateTime? CnicExpiryDate { get; set; } 
        public string? PassportNo { get; set; } 
        public DateTime? PassportExpiryDate { get; set; }
        public string? Nationality { get; set; } 
        public bool? OverSeas { get; set; }
        public string? CountryOfResidence { get; set; } 
        public string? CityOfResidence { get; set; } 
        public string? SourceOfInfo { get; set; }
        public string? MEMBERSHIPNO { get; set; }

        [DataType("decimal(18,2)")]
        public decimal? OutstandingBalance { get; set; }
        public string? NICOPNo { get; set; } 
        public string? POCNO { get; set; } 
        public string? BioMetircInfo { get; set; } 
        public string? CurrentAddress { get; set; } 
        public string? ResidenenceStatus { get; set; } 
        public string? PermanentAddress { get; set; } 
        public string? Vehicle { get; set; } 
        public string? Mobile { get; set; } 
        public string? Phone { get; set; } 
        public string? MothersMaidenName { get; set; } 
        public string? HomeNo { get; set; } 
        public string? WhatsAppNo { get; set; } 
        public string? OfficeNo { get; set; } 
        public string? ImoNo { get; set; } 
        public string? EmailId { get; set; } 
        public string? InstagramId { get; set; } 
        public string? LinkedInId { get; set; } 
        public string? FacebookId { get; set; } 
        public string? TwitterId { get; set; } 
        public string? Profession { get; set; } 
        public string? BussinessAddress { get; set; } 
        public string? BussinessTenoure { get; set; } 
        [DataType("decimal(18,2)")]
        public decimal Salary { get; set; }
        public string? TaxStatus { get; set; } 
        public string? NoOfDepartments { get; set; } 
        public string? RelationshipManager { get; set; } 
        public string? NTNNo { get; set; } 
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordKey { get; set; }
        public string? DocNum { get; set; } 
        public bool? SapPosting { get; set; }
        public string? DocEntry { get; set; }

        public string? OTP { get; set; }

        public DateTime? OTPExpiry { get; set; }

        public bool? IsMemberProfileRequested { get; set; }
        public bool? IsMemberProfileApproved { get; set ; }
        [MaxLength(100)]
        public string? PrfixMembershipNo { get; set; }
        [MaxLength(100)]
        public string? RepresentativeName { get; set; }
        [MaxLength(100)]
        public string? RepresentativeRelationshipWith { get; set; }
        [MaxLength(20)]
        public string? RepresentativeCnic { get; set; }


        //Navigation

        public virtual ICollection<MemberSocialStatus>? MemberSocialStatus { get; set; }
        public virtual ICollection<MemberInterest>? MemberInterest { get; set; }
        public virtual ICollection<MemberRelationshipHistery>? MemberRelationshipHistory { get; set; }
        public virtual ICollection<MemberNominees>? MemberNominees { get; set; }
        public virtual ICollection<MemberAttachments>? MemberAttachments { get; set; }
    }

    public class MemberSocialStatus: BaseModel
    {
        public int? MatchId { get; set;}
        public string? Description { get; set; } 
        public bool? IsEnabled { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
    }

    public class MemberInterest : BaseModel
    {
        public string? PropertyNature { get; set; } 
        public string? PropertyType { get; set; } 
        public string? Category { get; set; } 

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
    }

    public class MemberRelationshipHistery : BaseModel
    {
        public DateTime AlertDate { get; set; }
        public string? AlertNarration { get; set; } 
        public string? AlertType { get; set; } 
        public DateTime? ResolationDate { get; set; } 
        public string? ResolationDescription { get; set; } 

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
    }

    public class MemberNominees : BaseModel
    {
        public string? Name { get; set; } 
        public string? CNIC { get; set; } 
        public string? Relationship { get; set; } 
        public string? MobileNo { get; set; } 
        public string? Address { get; set; } 

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
    }

    public class MemberAttachments : BaseModel
    {
        public string? TargetPath { get; set; } 
        public string? FileName { get; set; } 
        public DateTime AttachmentDate { get; set; }
        public string? Description { get; set; } 
        public string? CopyToTargetDocument { get; set; } 

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
    }
}
