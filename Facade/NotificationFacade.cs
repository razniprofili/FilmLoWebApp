using Domain;
using Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    public partial class FilmLoFacade
    {
        public List<NotificationModel> GetAllMyNotifications(long currentUserId)
        {
            List<Notification> notifications = NotificationManager.GetNotifications(currentUserId) as List<Notification>;
          
            return notifications.Select(a => Mapper.Mapper.Map(a)).ToList();
        }

        public NotificationModel SendNotification(SendNotificationModel notificationModel, long currentUserId)
        {
            var addedNotification = NotificationManager.SendNotification(Mapper.Mapper.AutoMap<SendNotificationModel, Notification>(notificationModel), currentUserId);

            return Mapper.Mapper.Map(addedNotification);
        }

        public void DeleteNotification(long currentUserId, long notificationId)
        {
            NotificationManager.DeleteNotification(currentUserId, notificationId);
        }
    }
}
