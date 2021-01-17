using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface INotificationManager
    {
        public List<Notification> GetNotifications(long userId);
        public Notification SendNotification(Notification notification, long currentUserId);
    }
}
