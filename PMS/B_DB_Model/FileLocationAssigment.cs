using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FileLocationAssigment : BaseModel
    {
        public int? StockId { get; set; }
        public string? FileNo { get; set; }
        public string? RegistrationNo { get; set; }
        public string? Block { get; set; }
        public string? Rack { get; set; }
        public string? Row { get; set; }
    }
}
