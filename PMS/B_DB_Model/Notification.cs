using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Notification : BaseModel
    {
        public string Narration { get; set; }
        public string Type { get; set; }
        public string? SenderName { get; set; }
        public string? Designation { get; set; }
        public int Sender { get; set; }
        public bool IsViewed { get; set; }
        public ICollection<NotificationReceiver> Receivers { get; set; }
    }

    public class NotificationReceiver 
    {
        [Key]
        public int Id { get; set; }
        public string Receiver { get; set; }
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}
