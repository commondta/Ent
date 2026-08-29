using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SAPBilling : BaseModel
    {
        public string Server { get; set; } = String.Empty;
        public string DBName { get; set; } = String.Empty;
        public string DBUserName { get; set; } = String.Empty;
        public string DBPassword { get; set; } = String.Empty;
        public string SAPUser { get; set; } = String.Empty;
        public string SAPPassword { get; set; } = String.Empty;
        public int Series { get; set; }

        public string DBType { get; set; } = String.Empty;
    }
}
