using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Clearance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
      
        public string? RequestNo { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MemberCode { get; set; }
        public string? MemberName { get; set; }
        public string? CNIC { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public bool? is_active { get; set; }
        public bool? is_deleted { get; set; }

        public ICollection<ClearanceDetail> ClearanceDetails { get; set; }

    }
    public class ClearanceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Charges { get; set; }
        public string Department { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("ClearanceId")]
        public int ClearanceId { get; set; }
        public Clearance Clearance { get; set; }
    }
}
