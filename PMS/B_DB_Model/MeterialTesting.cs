using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MeterialTesting : BaseModel
    {
        public string? TestType { get; set; }
        public string? TestName { get; set; }
        public string? Amount { get; set; }
        public DateTime? TestDate { get; set; }
        public string? TestedBy { get; set; }
        public string? Result { get; set; }
        public string? Attachment { get; set; }
        public string? Remarks { get; set; }

        //Navigation Property
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }
    }
}
