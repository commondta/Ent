using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FileReceivingRegister : BaseModel
    {
        public int RegisterNo { get; set; }
        public string? Registration { get; set; }
        public string? Plot { get; set; }
        public string? Block { get; set; }
        public string? Area { get; set; }
        public string? SellerName { get; set; }
        public string? BuyerName { get; set; }
        public string? DocumentsNo { get; set; }
        public string? InternalNo { get; set; }
        public string? Remarks { get; set; }
    }
}
