using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class BookingBackLog: BaseModel
    {
        public int BookingId { get; set; }
        public int StockId { get; set; }
        public int BookingType { get; set; }
        public int BookingChargeId { get; set; }
        public bool BookingChargePosted { get; set; }
        public string ErrorMessage { get; set; }=string.Empty;
    }
}
