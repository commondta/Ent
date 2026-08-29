namespace HRMS_Web.Services.NotificationService
{
    public interface INotificationService
    {
        Task<string> SendOfferNotificationAsync(string title, string message, string topic = "offers");
    }
}
