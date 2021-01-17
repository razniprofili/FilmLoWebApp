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
            List<Notification> notifications = NotificationcManager.GetNotifications(currentUserId) as List<Notification>;
          
            return notifications.Select(a => Mapper.Mapper.Map(a)).ToList();
        }

        public NotificationModel SendNotification(SendNotificationModel notificationModel, long currentUserId)
        {
            var addedNotification = NotificationcManager.SendNotification(Mapper.Mapper.AutoMap<SendNotificationModel, Notification>(notificationModel), currentUserId);

            return Mapper.Mapper.Map(addedNotification);
        }
    }
}
