using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class NotificationManager : INotificationManager
    {
        public List<Notification> GetNotifications(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var notifications = uow.NotificationRepository.Find(m => m.UserRecipientId == userId, "UserSender").ToList();

                return notifications;

            }
        }

        public Notification SendNotification(Notification notification, long currentUserId)
        {
            using (var uow = new UnitOfWork())
            {
                var userRecipient = uow.UserRepository.FirstOrDefault(a => a.Id == notification.UserRecipientId);
                ValidationHelper.ValidateNotNull(userRecipient);

                var friendship = uow.FriendshipRepository.FirstOrDefault(f => (f.UserSenderId == currentUserId && f.UserRecipientId == notification.UserRecipientId && f.StatusCodeID == 'A') || (f.UserSenderId == notification.UserRecipientId && f.UserRecipientId == currentUserId && f.StatusCodeID == 'A'));
                ValidationHelper.ValidateNotNull(friendship);

                notification.SendingDate = DateTime.Now;
                notification.UserSenderId = currentUserId;

                var userSender = uow.UserRepository.FirstOrDefault(a => a.Id == currentUserId);
                notification.UserSender = userSender;
                notification.UserRecipient = userRecipient;

                uow.NotificationRepository.Add(notification);
                uow.Save();

                return notification;
            }
        }
    
        public void DeleteNotification(long currentUserId, long notificationId)
        {
            using (var uow = new UnitOfWork())
            {
                // proveravamo da li postoji notifikacija, po idju usera i idju notifikacije koji se brise
                var notificationToDelete = uow.NotificationRepository.FirstOrDefault(p => p.UserRecipientId == currentUserId && p.Id == notificationId);
                ValidationHelper.ValidateNotNull(notificationToDelete);

                uow.NotificationRepository.Delete(notificationToDelete);
                uow.Save();
            }
        }
    
    }
}
