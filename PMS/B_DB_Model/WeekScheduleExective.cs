using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class WeekScheduleExective : BaseModel
    {
        public string DayOfWeek { get; set; }
        public string Hour { get; set; }
        public string Mintues { get; set; }
        public string? Remarks { get; set; }
    }
}
