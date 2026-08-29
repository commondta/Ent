using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class LawyerData : BaseModel
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

        //Navigation
        [ForeignKey("CaseCategoryId")]
        public int? CaseCategoryId { get; set; }
        public virtual CaseCategory? CaseCategory { get; set; }

        [ForeignKey("CaseTypeId")]
        public int? CaseTypeId { get; set; }
        public virtual CaseType? CaseType { get; set; }

    }
}
