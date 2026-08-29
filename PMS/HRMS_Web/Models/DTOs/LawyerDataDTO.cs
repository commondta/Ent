using B_DB_Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_Web.Models.DTOs
{
    public class LawyerDataDTO
    {
        public string LawyerCode { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string CNIC { get; set; } = String.Empty;
        public string PhoneNo { get; set; } = String.Empty;
        public string Nationality { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Jurisdiction { get; set; } = String.Empty;
        public int? CaseCategoryId { get; set; }
        public int? CaseTypeId { get; set; }
    }
}
