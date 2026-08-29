using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace B_DB_Model
{
    public class DealerProfile : BaseModel
    {
        public string? DealerCode { get; set; }
        public string? DealerStatus { get; set; }
        public string? CNIC { get; set; }
        public string? ResidentialAddress { get; set; }
        public string? DealerCategory { get; set; }
        public string? EstateName { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string? Nationality { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public double? OutstandingBalance { get; set; }
        public double? OutstandingAdvance { get; set; }
        public string? Picture { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICollection<EstateDetail>? EstateDetail { get; set; }
        public ICollection<Attachments>? Attachments { get; set; }
    }

    public class EstateDetail : BaseModel
    {

        public string Designation { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string TelephoneNo { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Remarks { get; set; }= string.Empty;

        [ForeignKey("DealerProfileId")]
        public int? DealerProfileId { get; set; }
        public DealerProfile? DealerProfile { get; set; }
    }

    public class Attachments : BaseModel
    {
        public string TargetPath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime AttachmentDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CopyToTargetDocument { get; set; } = string.Empty;

        [ForeignKey("DealerProfileId")]
        public int? DealerProfileId { get; set; }
        public DealerProfile? DealerProfile { get; set; }
    }
}
