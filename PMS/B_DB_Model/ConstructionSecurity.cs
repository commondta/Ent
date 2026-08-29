using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
  
    public class ConstructionSecurity : BaseModel
    {
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [MaxLength(200)]
        public string ContractorName { get; set; }
        [MaxLength(20)]
        public string MobileNumber { get; set; }
        [MaxLength(20)]
        public string CNIC { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(200)]
        public string SurveyorName { get; set; }
        [MaxLength(20)]
        public string SurveyorNumber { get; set; }
        [MaxLength(20)]
        public string SurveyorCNIC { get; set; }
        [MaxLength(200)]
        public string? SurveyorAddress { get; set; }
        
        public string? Remarks { get; set; }

        public ICollection<ConstructionSecurityLabour>? ConstructionSecurityLabour { get; set; }
        public ICollection<ConstructionSecurityAttachment>? ConstructionSecurityAttachment { get; set; }
    }  
    
    
    public class ConstructionSecurityLabour : BaseModel
    {
        
        public string Name { get; set; }
        public string CNIC { get; set; }
        public string? CNICAttachment { get; set; }
        public DateTime? GatePassValidity { get; set; }

        [ForeignKey("ConstructionSecurityId")]
        public int ConstructionSecurityId { get; set; }
       
        public ConstructionSecurity? ConstructionSecurity { get; set; }
    }

    public class ConstructionSecurityAttachment : BaseModel
    {
        public string AttachmentPath { get; set; }
        public string AttachmentName { get; set; }
        public string Attachment { get; set; }
        public DateTime? AttachmentDate { get; set; }
        public string? Remarks { get; set; }


        [ForeignKey("ConstructionSecurityId")]
        public int ConstructionSecurityId { get; set; }

        public ConstructionSecurity? ConstructionSecurity { get; set; }
    }
}
