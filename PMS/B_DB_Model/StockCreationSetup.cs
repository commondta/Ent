using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class StockCreationSetup : BaseModel
    {
        public string? ConstrucationStatus { get; set; }
        public bool? PossessionStatus { get; set; }
    }
}
