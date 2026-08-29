
using FirebaseAdmin.Messaging;

namespace HRMS_Web.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        public async Task<string> SendOfferNotificationAsync(string title, string message, string topic = "offers")
        {
            var notification = new Notification
            {
                Title = title,
                Body = message,
            };

            var messageToSend = new Message
            {
                Notification = notification,
                Topic = topic,
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(messageToSend);
        }
    }
}
