using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MeterReading : BaseModel
    {
        public string Month { get; set; } = string.Empty;
        public string MeterReadingOfficer { get; set; } = string.Empty;
        public string ReadingFor { get; set; } = string.Empty;

        public virtual ICollection<ReadingDetail>? ReadingDetail { get; set; }
    }

    public class ReadingDetail : BaseModel
    {
        public string MeterNo { get; set; } = string.Empty;
        public string? PropertyNo { get; set; }
        public decimal LastReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal UnitsConsumed { get; set; }
        public string Picture { get; set; } = string.Empty;
        public DateTime? ReadingDate { get; set; }

        [NotMapped]
        public int SaleTax { get; set; }

        [NotMapped]
        public int FuelAdjustedUnits { get; set; }

        //Navigation
        [ForeignKey("MeterReadingId")]
        public int? MeterReadingId { get; set; }
        public virtual MeterReading? MeterReading { get; set; }

        [ForeignKey("ReadingOfficerId")]
        public int? ReadingOfficerId { get; set; }
        public virtual ReadingOfficer? ReadingOfficer { get; set; }

    }
}
