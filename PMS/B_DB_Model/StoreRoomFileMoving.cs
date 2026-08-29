using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class StoreRoomFileMoving : BaseModel
    {
        public int StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int PMSUserId { get; set; }
        public PMSUser? PMSUser { get; set; }
        public bool IsFileClosed { get; set; }
        public string? Remarks { get; set; }
        public DateTime? ExpectedReceivingDate { get; set; }
        public string? PageOutIn { get; set; }
        public bool IsRecordRoom { get; set; } = false;

        [NotMapped]
        public int? PageOut { get; set; }
    }
}
