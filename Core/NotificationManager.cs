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
        #region Fields
        private readonly IUnitOfWork _uow;
        #endregion

        #region Constructor
        public NotificationManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        public List<Notification> GetNotifications(long userId)
        {
            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var notifications = _uow.Notification.Find(m => m.UserRecipientId == userId, "UserSender").ToList();

            return notifications;
        }

        public Notification SendNotification(Notification notification, long currentUserId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            var userRecipient = _uow.Users.FirstOrDefault(a => a.Id == notification.UserRecipientId, "");
            ValidationHelper.ValidateNotNull(userRecipient);

            var friendship = _uow.Friendships.FirstOrDefault(f =>
            (f.UserSenderId == currentUserId && f.UserRecipientId == notification.UserRecipientId && f.StatusCodeID == 'A')
            || (f.UserSenderId == notification.UserRecipientId && f.UserRecipientId == currentUserId && f.StatusCodeID == 'A'), "");
            ValidationHelper.ValidateNotNull(friendship);

            notification.SendingDate = DateTime.Now;
            notification.UserSenderId = currentUserId;
            notification.UserSender = user; // curr user is sender
            notification.UserRecipient = userRecipient;

            _uow.Notification.Add(notification, "");
            _uow.Save();

            return notification;

        }

        public void DeleteNotification(long currentUserId, long notificationId)
        {
            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == currentUserId, "");
            ValidationHelper.ValidateNotNull(user);

            // proveravamo da li postoji notifikacija, po idju usera i idju notifikacije koji se brise
            var notificationToDelete = _uow.Notification.FirstOrDefault(p => p.UserRecipientId == currentUserId && p.Id == notificationId, "");
            ValidationHelper.ValidateNotNull(notificationToDelete);

            _uow.Notification.Delete(notificationToDelete);
            _uow.Save();

        }
        #endregion

    }
}
