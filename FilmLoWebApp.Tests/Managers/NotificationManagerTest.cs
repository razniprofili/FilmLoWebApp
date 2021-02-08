using Common.Exceptions;
using Core;
using Data;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmLoWebApp.Tests.Managers
{
    public class NotificationManagerTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private INotificationManager _manager;
        private User fakeCurrentUser;
        private User fakeOtherUser;
        private Notification fakeNotification;
        private Notification notificationToSend;
        private List<Notification> fakeListResult;
        private Friendship fakeFriendship;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _manager = new NotificationManager(_uowMock.Object);
            fakeCurrentUser = new User
            {
                Id = 1,
                Name = "user1",
                Surname = "user1",
                Password = "pass1",
                Picture = "picture1",
                Email = "email1",
                NotificationsReceived = null,
                NotificationsSent = null,
                FriendsReceived = null,
                FriendsSent = null,
                SavedMovies = null,
                WatchedMovies = null
            };

            fakeOtherUser = new User
            {
                Id = 2,
                Name = "user2",
                Surname = "user2",
                Password = "pass2",
                Picture = "picture2",
                Email = "email2",
                NotificationsReceived = null,
                NotificationsSent = null,
                FriendsReceived = null,
                FriendsSent = null,
                SavedMovies = null,
                WatchedMovies = null
            };

            fakeListResult = new List<Notification>
            {
                new Notification 
                {
                    Id = 1, 
                    SendingDate = DateTime.Now, 
                    Text = "Notification 1", 
                    UserRecipientId = 1, 
                    UserRecipient = fakeCurrentUser,
                    UserSenderId = 2,
                    UserSender = fakeOtherUser
                },
                new Notification 
                {
                    Id = 2,
                    SendingDate = DateTime.Now,
                    Text = "Notification 2",
                    UserRecipientId = 1,
                    UserRecipient = fakeCurrentUser,
                    UserSenderId = 2,
                    UserSender = fakeOtherUser
                },
            };

            fakeNotification = new Notification
            {
                Id = 1,
                SendingDate = DateTime.Now,
                Text = "Notification 1",
                UserRecipientId = 1,
                UserRecipient = fakeCurrentUser,
                UserSenderId = 2,
                UserSender = fakeOtherUser
            };

            fakeFriendship = new Friendship
            {
                UserSenderId = 1,
                UserSender = fakeCurrentUser,
                UserRecipientId = 2,
                UserRecipient = fakeOtherUser,
                FriendshipDate = new DateTime(2020, 12, 5),
                StatusCode = null,
                StatusCodeID = 'A'
            };

            notificationToSend = new Notification // user sends only notification and UserRecipientId!
            {                                         // other fields set in function!
                Id = 0,
                SendingDate = new DateTime(),
                Text = "Notification 1",
                UserRecipientId = 2,
                UserRecipient = null,
                UserSenderId = 0,
                UserSender = null
            };
        }

        [Test]
        public void GetNotifications()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeCurrentUser);

            _uowMock.Setup(uow => uow.Notification.Find(stat => stat.UserRecipientId == 1, "UserSender"))
                .Returns(fakeListResult.AsQueryable());

            var resultList = _manager.GetNotifications(fakeCurrentUser.Id);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.ToList().Count());
        }

        [Test]
        public void GetNotificationsUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetNotifications(1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void SendNotification()
        {
          var  notificationToSend = new Notification // user sends only notification and UserRecipientId!
            {                                         // other fields set in function!
                Id = 0,
                SendingDate = new DateTime(),
                Text = "Notification 1",
                UserRecipientId = 2,
                UserRecipient = null,
                UserSenderId = 0,
                UserSender = null
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeCurrentUser);

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == notificationToSend.UserRecipientId, ""))
                .Returns(fakeOtherUser);

            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f =>
            (f.UserSenderId == 1 && f.UserRecipientId == notificationToSend.UserRecipientId && f.StatusCodeID == 'A')
            || (f.UserSenderId == notificationToSend.UserRecipientId && f.UserRecipientId == 1 && f.StatusCodeID == 'A'), ""))
                .Returns(fakeFriendship);

            _uowMock.Setup(uow => uow.Notification.Add(notificationToSend, ""))
                .Returns(notificationToSend);

            _uowMock.Setup(uow => uow.Save()).Callback(() => { notificationToSend.Id = 11; });
                
            var result =  _manager.SendNotification(notificationToSend, 1);

            _uowMock.Verify(uow => uow.Notification.Add(notificationToSend, ""), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());

            Assert.AreEqual(11, result.Id);
            Assert.AreEqual(fakeCurrentUser.Id, result.UserSenderId);
            Assert.AreEqual(fakeCurrentUser, result.UserSender);
            Assert.AreEqual(fakeOtherUser, result.UserRecipient);
        }

        [Test]
        public void SendNotificationCurrentUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.SendNotification(notificationToSend, 1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void SendNotificationRecipientUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeCurrentUser);

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == notificationToSend.UserRecipientId, ""))
                .Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.SendNotification(notificationToSend, 1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void SendNotificationFriendshiprNotExists()
        {

            var notificationToSend = new Notification // user sends only notification and UserRecipientId!
            {                                         // other fields set in function!
                Id = 0,
                SendingDate = new DateTime(),
                Text = "Notification 1",
                UserRecipientId = 2,
                UserRecipient = null,
                UserSenderId = 0,
                UserSender = null
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeCurrentUser);

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == notificationToSend.UserRecipientId, ""))
                .Returns(fakeOtherUser);

            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f =>
            (f.UserSenderId == 1 && f.UserRecipientId == notificationToSend.UserRecipientId && f.StatusCodeID == 'A')
            || (f.UserSenderId == notificationToSend.UserRecipientId && f.UserRecipientId == 1 && f.StatusCodeID == 'A'), ""))
                .Returns((Friendship)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.SendNotification(notificationToSend, 1); });
            Assert.That(ex.Message, Is.EqualTo("Friendship not exist!"));

        }


        [Test]
        public void DeleteNotification()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeCurrentUser);

            _uowMock.Setup(uow => uow.Notification.FirstOrDefault(p => p.UserRecipientId == 1 && p.Id == 1, ""))
                .Returns(fakeNotification);

             _manager.DeleteNotification(1, 1);

            _uowMock.Verify(uow => uow.Notification.Delete(It.IsAny<Notification>()), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
        }

        [Test]
        public void DeleteNotificationUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.DeleteNotification(1, 1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void DeleteNotificationNotificationNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeCurrentUser);

            _uowMock.Setup(uow => uow.Notification.FirstOrDefault(p => p.UserRecipientId == 1 && p.Id == 1, ""))
                .Returns((Notification)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.DeleteNotification(1, 1); });
            Assert.That(ex.Message, Is.EqualTo("Notification not exist!"));

        }

    }
}
