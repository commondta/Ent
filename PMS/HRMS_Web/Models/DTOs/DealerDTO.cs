using B_DB_Model;
using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class DealerDTO
    {
        public int Id { get; set; }

        [Required]
        public string PictureBase64 { get; set; }

        [Required]
        [MaxLength(20)]
        public string? DealerCode { get; set; }

        [Required]
        [MaxLength(20)]
        public string SerialNo { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegistrationFee { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocumentStatus { get; set; }

        [Required]
        [MaxLength(20)]
        public string CNIC { get; set; }

        [Required]
        [MaxLength(500)]
        public string ResidentialAddress { get; set; }

        [Required]
        public int DealerCategory { get; set; }

        [Required]
        [MaxLength(200)]
        public string PrincipalOwner { get; set; }

        [Required]
        [MaxLength(200)]
        public string EstateName { get; set; }

        [Required]
        [MaxLength(200)]
        public string EstateAddress { get; set; }

        [Required]
        public DateTime RenewalDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nationality { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        //Navigation 
        public List<DealerEstateDeatail> DealerEstateDeatail { get; set; }
        public List<DealerAttachments> DealerAttachments { get; set; }
    }
}
