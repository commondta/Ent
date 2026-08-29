using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace HRMS_Web.Services.AlertService
{
    public class AlertService : IAlertService
    {
        private readonly DataBase_Context _db;
        public AlertService(DataBase_Context db)
        {
            _db = db;
        }
        public async Task<bool> PushAlert(int formId, string narration)
        {
            var users = _db.FormAlertUsers.Where(x=>x.FormAlert.FormId == formId).ToList();

            if(users.Count() > 0 )
            {
                Notification notification = new Notification();

                notification.Narration = narration;
                notification.Sender = 1;
                notification.SenderName = "System User";
                notification.Designation = "System";
                notification.Type = "System Alert";
                notification.IsViewed = false;
                notification.CreatedBy = 1;
                notification.CreatedOn = DateTime.Now;
                notification.IsActive = true;
                notification.IsDeleted = false;

                List<NotificationReceiver> notificationReceivers = new List<NotificationReceiver>();

                foreach (var item in users)
                {
                    string userId = item.UserId.ToString();

                    if (!string.IsNullOrEmpty(userId))
                    {
                        NotificationReceiver notificationReceiver = new NotificationReceiver()
                        {
                            Receiver = userId,
                        };

                        notificationReceivers.Add(notificationReceiver);
                    }
                }

                notification.Receivers = notificationReceivers;

                _db.Notifications.Add(notification);
                _db.SaveChanges();

                return true;

            }
            return false;
        }

        public bool GetNDC()
        {
            var todayMinus10Days = DateTime.Now.Date.AddDays(-10);

            var ndcs = _db.NDCRequestForMember
                          .Where(x => x.IsCanceled != true &&
                                      x.IsRead != true &&
                                      x.IsRequestedClosed != true &&
                                      x.ValidityDate == todayMinus10Days)
                          .Include(x => x.StockCreation)
                          .Include(x => x.TransferType)
                          .Include(x => x.MemberProfile)
                          .ToList();

            if (ndcs.Count > 0)
            {
                var users = _db.FormAlertUsers.Where(x => x.FormAlert.FormId == 4).ToList();

                foreach (var nd in ndcs)
                {

                    string narration = $"{nd.TransferType.Description} of property {nd.StockCreation.PropertyNo} which is owned by {nd.MemberProfile.MemberName} is scheduled on {nd.SlotHour} : {nd.SlotMintues}";

                    if (users.Count() > 0)
                    {
                        Notification notification = new Notification
                        {
                            Narration = narration,
                            Sender = 1,
                            SenderName = "System User",
                            Designation = "System",
                            Type = "System Alert",
                            IsViewed = false,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false
                        };

                        List<NotificationReceiver> notificationReceivers = new List<NotificationReceiver>();

                        foreach (var item in users)
                        {
                            string userId = item.UserId.ToString();

                            if (!string.IsNullOrEmpty(userId))
                            {
                                NotificationReceiver notificationReceiver = new NotificationReceiver
                                {
                                    Receiver = userId
                                };
                                notificationReceivers.Add(notificationReceiver);
                            }
                        }

                        notification.Receivers = notificationReceivers;
                         _db.Notifications.Add(notification);

                        nd.IsRead = true;
                    }
                }
                _db.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
