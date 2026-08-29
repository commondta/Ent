namespace HRMS_Web.Models.DTOs
{
    public class NotificationDTO
    {
        public string[] SelectedUsers { get; set; }
        public string Narration { get; set; }
        public string Type { get; set; }
        public int Sender { get; set; }
        public string? SenderName { get; set; }
        public string? Designation { get; set; }
    }
}
