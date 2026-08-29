using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FileVerificationNDC1 : BaseModel
    {
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }
       
        public string? Remarks { get; set; }
       
        public virtual ICollection<FileVerificationNDC1PowerOfAttorey>? FileVerificationNDC1PowerOfAttorey { get; set; }
        public virtual ICollection<FileVerificationNDC1CheckList>? FileVerificationNDC1CheckList { get; set; }
        public virtual ICollection<FileVerificationNDC1Attachments>? FileVerificationNDC1Attachments { get; set; }
    }

    public class FileVerificationNDC1PowerOfAttorey 
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Cnic { get; set; }

        [ForeignKey("FileVerificationNDC1Id")]
        public int? FileVerificationNDC1Id { get; set; }
        public FileVerificationNDC1? FileVerificationNDC1 { get; set; }
    }

    public class FileVerificationNDC1CheckList 
    {
        [Key]
        public int Id { get; set; }
        public string? Department { get; set; }
        public string? Remarks { get; set; }
        public string? AlertNarration { get; set; }
        public string? Date { get; set; }

        [ForeignKey("FileVerificationNDC1Id")]
        public int? FileVerificationNDC1Id { get; set; }
        public FileVerificationNDC1? FileVerificationNDC1 { get; set; }
    }
    public class FileVerificationNDC1Attachments 
    {
        [Key]
        public int Id { get; set; }
        public string DoucmentName { get; set; }
        public string Document { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("FileVerificationNDC1Id")]
        public int? FileVerificationNDC1Id { get; set; }
        public FileVerificationNDC1? FileVerificationNDC1 { get; set; }
    }
}
