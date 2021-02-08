using Common.Exceptions;
using Common.ResourceParameters;
using Core;
using Core.Services;
using Data;
using Domain;
using Models.User;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmLoWebApp.Tests.Managers
{
    public class FriendshipManagerTest
    {
        #region Setup

        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyMappingService> _propertyMappingMock;
        private Mock<IPropertyCheckerService> _propertyCheckerMock;
        private IFriendshipManager _manager;
        private User fakeUser;
        private User fakeUserFriend;
        private User fakeUserFriendTwo;
        private List<User> fakeListResult;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _propertyMappingMock = new Mock<IPropertyMappingService>();
            _propertyCheckerMock = new Mock<IPropertyCheckerService>();
            _manager = new FriendshipManager(_propertyMappingMock.Object, _propertyCheckerMock.Object, _uowMock.Object);
            fakeUser = new User
            {
                Id = 1,
                Name = "user",
                Surname = "user",
                Password = "pass",
                Picture = "picture",
                Email = "email",
                NotificationsReceived = null,
                NotificationsSent = null,
                FriendsReceived = null,
                FriendsSent = null,
                SavedMovies = null,
                WatchedMovies = null
            };

            fakeUserFriend = new User
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

            fakeUserFriendTwo = new User
            {
                Id = 3,
                Name = "user3",
                Surname = "user3",
                Password = "pass3",
                Picture = "picture3",
                Email = "email3",
                NotificationsReceived = null,
                NotificationsSent = null,
                FriendsReceived = null,
                FriendsSent = null,
                SavedMovies = null,
                WatchedMovies = null
            };

            fakeListResult = new List<User>
            {
                new User
            {
                Id = 1,
                Name = "user",
                Surname = "user",
                Password = "pass",
                Picture = "picture",
                Email = "email",
                NotificationsReceived = null,
                NotificationsSent = null,
                FriendsReceived = null,
                FriendsSent = null,
                SavedMovies = null,
                WatchedMovies = null
            },
              new User
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
            }
            };
        }
        #endregion

        #region Tests
        [Test]
        public void GetAllFriendsNoParameters()
        {
            long currentUserId = 1;

            var friends = new List<Friendship>
            {
                new Friendship
            {
                UserRecipientId = 1,
                UserRecipient = fakeUser,
                UserSenderId = 2,
                UserSender = fakeUserFriend,
                FriendshipDate = new DateTime(2021,1,12),
                StatusCodeID = 'A'

            },
              new Friendship
            {
                UserRecipientId = 3,
                UserRecipient = fakeUserFriendTwo,
                UserSenderId = 1,
                UserSender = fakeUser,
                FriendshipDate = new DateTime(2020,11,22),
                StatusCodeID = 'A'
            }
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Friendships.Find(m => (m.UserSenderId == currentUserId && m.StatusCodeID == 'A')
            || (m.UserRecipientId == currentUserId && m.StatusCodeID == 'A'), "")).Returns(friends.AsQueryable());

            long firstFriendId = 2;
            long secondFriendId = 3;

            _uowMock.Setup(uow => uow.Users.GetById(firstFriendId)).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Users.GetById(secondFriendId)).Returns(fakeUserFriendTwo);

            var usersToReturn = _manager.GetAllMyFriends(currentUserId) as List<User>;

            Assert.IsNotNull(usersToReturn);
            Assert.AreEqual(2, usersToReturn.Count());

        }

        [Test]
        public void GetAllFriendsWithParameters()
        {
            long currentUserId = 1;

            var friends = new List<Friendship>
            {
                new Friendship
            {
                UserRecipientId = 1,
                UserRecipient = fakeUser,
                UserSenderId = 2,
                UserSender = fakeUserFriend,
                FriendshipDate = new DateTime(2021,1,12),
                StatusCodeID = 'A'

            },
              new Friendship
            {
                UserRecipientId = 3,
                UserRecipient = fakeUserFriendTwo,
                UserSenderId = 1,
                UserSender = fakeUser,
                FriendshipDate = new DateTime(2020,11,22),
                StatusCodeID = 'A'
            }
            };

            ResourceParameters parameters = new ResourceParameters
            {
                Fields = "Id, name",
                OrderBy = "name",
                PageNumber = 1,
                PageSize = 10,
                SearchQuery = null
            };

            var dictionaryToReturn = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Name", new PropertyMappingValue(new List<string>() { "Name"}) }
            };

            _propertyMappingMock.Setup(prop => prop.ValidMappingExistsFor<UserModel, User>
               (parameters.OrderBy)).Returns(true);

            _propertyMappingMock.Setup(prop => prop.GetPropertyMapping<UserModel, User>
                ()).Returns(dictionaryToReturn);

            _propertyCheckerMock.Setup(check => check.TypeHasProperties<UserModel>
                (parameters.Fields)).Returns(true);

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Friendships.Find(m => (m.UserSenderId == currentUserId && m.StatusCodeID == 'A')
            || (m.UserRecipientId == currentUserId && m.StatusCodeID == 'A'), "")).Returns(friends.AsQueryable());

            long firstFriendId = 2;
            long secondFriendId = 3;

            _uowMock.Setup(uow => uow.Users.GetById(firstFriendId)).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Users.GetById(secondFriendId)).Returns(fakeUserFriendTwo);

            var usersToReturn = _manager.GetAllMyFriends(currentUserId, parameters) as List<User>;

            Assert.IsNotNull(usersToReturn);
            Assert.AreEqual(2, usersToReturn.Count());

        }
        [Test]
        public void AddFriendship()
        {
            var friendshipToAdd = new Friendship
            {
                UserRecipientId = 2
            };

            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == friendshipToAdd.UserRecipientId, "")).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f => (f.UserSenderId == currentUserId && f.UserRecipientId == friendshipToAdd.UserRecipientId)
            || (f.UserSenderId == friendshipToAdd.UserRecipientId && f.UserRecipientId == currentUserId), "")).Returns((Friendship)null);

            _uowMock.Setup(uow => uow.Friendships.Add(friendshipToAdd, "")).Returns(friendshipToAdd);
            _uowMock.Setup(uow => uow.Save()).Callback(() => { friendshipToAdd.UserSenderId = 1; });

            var result = _manager.Add(friendshipToAdd, currentUserId);

            _uowMock.Verify(uow => uow.Friendships.Add(friendshipToAdd, ""), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());

            Assert.AreEqual('R', friendshipToAdd.StatusCodeID);
            Assert.AreEqual(fakeUser, friendshipToAdd.UserSender);
            Assert.AreEqual(1, friendshipToAdd.UserSenderId);
            Assert.AreEqual(fakeUserFriend, friendshipToAdd.UserRecipient);
        }

        [Test]
        public void AddFriendshipExist()
        {
            var friendshipToAdd = new Friendship
            {
                UserRecipientId = 2
            };

            var friendship = new Friendship
            {
                UserSenderId = 1,
                UserSender = fakeUser,
                UserRecipientId = 2,
                UserRecipient = fakeUserFriend,
                FriendshipDate = new DateTime(2020, 1, 15),
                StatusCodeID = 'A'
            };

            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == friendshipToAdd.UserRecipientId, "")).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f => (f.UserSenderId == currentUserId && f.UserRecipientId == friendshipToAdd.UserRecipientId)
            || (f.UserSenderId == friendshipToAdd.UserRecipientId && f.UserRecipientId == currentUserId), ""))
                .Returns(friendship);


            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Add(friendshipToAdd, currentUserId); });
            Assert.That(ex.Message, Is.EqualTo("Friendship currently exists!"));
        }

        [Test]
        public void DeleteFriend()
        {

            long currentUserId = 1;
            long friendId = 2;

            var friendship = new Friendship
            {
                UserSenderId = 1,
                UserSender = fakeUser,
                UserRecipientId = 2,
                UserRecipient = fakeUserFriend,
                FriendshipDate = new DateTime(2020, 1, 15),
                StatusCodeID = 'A'
            };

            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f => (f.UserSenderId == currentUserId && f.UserRecipientId == friendId && f.StatusCodeID == 'A')
            || (f.UserSenderId == friendId && f.UserRecipientId == currentUserId && f.StatusCodeID == 'A'), ""))
                .Returns(friendship);

            _manager.DeleteFriend(friendId, currentUserId);

            _uowMock.Verify(uow => uow.Friendships.Delete(It.IsAny<Friendship>()), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
        }
        [Test]
        public void AcceptRequest()
        {
            long currentUserId = 1;
            long friendId = 2;

            var friendship = new Friendship
            {
                UserSenderId = 1,
                UserSender = fakeUser,
                UserRecipientId = 2,
                UserRecipient = fakeUserFriend,
                FriendshipDate = new DateTime(2020, 1, 15),
                StatusCodeID = 'R'
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == friendId, "")).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f => f.UserSenderId == friendId && f.UserRecipientId == currentUserId
                    && f.StatusCodeID == 'R', "")).Returns(friendship);

            _uowMock.Setup(uow => uow.Friendships.Update(friendship, friendId, currentUserId)).Returns(friendship);

            var result = _manager.AcceptRequest(currentUserId, friendId);

            _uowMock.Verify(uow => uow.Friendships.Update(friendship, friendId, currentUserId), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());

            Assert.AreEqual('A', friendship.StatusCodeID);
        }

        [Test]
        public void DeclineRequest()
        {
            long currentUserId = 1;
            long friendId = 2;

            var friendship = new Friendship
            {
                UserSenderId = 1,
                UserSender = fakeUser,
                UserRecipientId = 2,
                UserRecipient = fakeUserFriend,
                FriendshipDate = new DateTime(2020, 1, 15),
                StatusCodeID = 'R'
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == friendId, "")).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f => f.UserSenderId == friendId && f.UserRecipientId == currentUserId
                    && f.StatusCodeID == 'R', "")).Returns(friendship);

            _manager.DeclineRequest(currentUserId, friendId);

            _uowMock.Verify(uow => uow.Friendships.Delete(friendship), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
        }

        [Test]
        public void MyFriendRequests()
        {
            long currentUserId = 1;

            var requests = new List<Friendship>
            {
                new Friendship
            {
                UserRecipientId = 1,
                UserRecipient = fakeUser,
                UserSenderId = 2,
                UserSender = null,
                FriendshipDate = new DateTime(2021,1,12),
                StatusCodeID = 'R'

            },
              new Friendship
            {
                UserRecipientId = 1,
                UserRecipient = fakeUser,
                UserSenderId = 3,
                UserSender = null,
                FriendshipDate = new DateTime(2020,11,22),
                StatusCodeID = 'R'
            }
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(u => u.Id == 2, "")).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(u => u.Id == 3, "")).Returns(fakeUserFriendTwo);
            _uowMock.Setup(uow => uow.Friendships.Find(f => f.UserRecipientId == currentUserId && f.StatusCodeID == 'R', ""))
                .Returns(requests.AsQueryable());

            var result = _manager.FriendRequests(currentUserId);

            Assert.NotNull(result);
            Assert.AreEqual(requests.Count(), result.Count());

        }

        [Test]
        public void MySentFriendRequests()
        {
            long currentUserId = 1;

            var requests = new List<Friendship>
            {
                new Friendship
            {
                UserRecipientId = 2,
                UserRecipient = null,
                UserSenderId = 1,
                UserSender = fakeUser,
                FriendshipDate = new DateTime(2021,1,12),
                StatusCodeID = 'R'

            },
              new Friendship
            {
                UserRecipientId = 3,
                UserRecipient = null,
                UserSenderId = 1,
                UserSender = fakeUser,
                FriendshipDate = new DateTime(2020,11,22),
                StatusCodeID = 'R'
            }
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(u => u.Id == 2, "")).Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(u => u.Id == 3, "")).Returns(fakeUserFriendTwo);
            _uowMock.Setup(uow => uow.Friendships.Find(f => f.UserSenderId == currentUserId && f.StatusCodeID == 'R', ""))
                .Returns(requests.AsQueryable());

            var result = _manager.SentFriendRequests(currentUserId);

            Assert.NotNull(result);
            Assert.AreEqual(requests.Count(), result.Count());
        }

        [Test]
        public void MySentFriendRequestsNoRequests()
        {
            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Friendships.Find(f => f.UserSenderId == currentUserId && f.StatusCodeID == 'R', ""))
                .Returns(new List<Friendship>().AsQueryable());

            var result = _manager.SentFriendRequests(currentUserId);

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

    }
}
