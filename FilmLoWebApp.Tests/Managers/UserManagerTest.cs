using Common.Exceptions;
using Core;
using Core.Services;
using Data;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Helpers;
using System.Linq;
using Common.ResourceParameters;
using Models.User;

namespace FilmLoWebApp.Tests.Managers
{
    public class UserManagerTest
    {
        #region Setup
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyMappingService> _propertyMappingMock;
        private Mock<IPropertyCheckerService> _propertyCheckerMock;
        private Mock<IPasswordHelper> _passHelperMock;
        private IUserManager _manager;
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
            _passHelperMock = new Mock<IPasswordHelper>();
            _manager = new UserManager(_propertyMappingMock.Object, _propertyCheckerMock.Object, _uowMock.Object,
                _passHelperMock.Object);
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

        #region Register user
        [Test]
        public void RegisterUser()
        {
            var newUser = new User
            {
                Id = 0,
                Email = "user1@gmail.com",
                Password = "pass",
                Name = "name1",
                Surname = "surname1"
            };

            _uowMock.Setup(uow => uow.Users.Any(u => u.Email == newUser.Email)).Returns(false);

            _passHelperMock.Setup(pass => pass.CreateHash(newUser.Password))
                .Returns("eyybofhgfdh7877esiy668he0qauda0tt0r0shf0sa");

            _uowMock.Setup(uow => uow.Users.Add(newUser, "")).Returns(newUser);

            _uowMock.Setup(uow => uow.Save()).Callback(() => { newUser.Id = 11; });

            var resultUser = _manager.Register(newUser);

            _uowMock.Verify(uow => uow.Users.Add(newUser, ""), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
            Assert.IsNotNull(resultUser);
            Assert.AreEqual(11, resultUser.Id);
            Assert.AreEqual("eyybofhgfdh7877esiy668he0qauda0tt0r0shf0sa", resultUser.Password);
        }

        [Test]
        public void RegisterUserUserExists()
        {
            var newUser = new User
            {
                Id = 0,
                Email = "user1@gmail.com",
                Password = "pass",
                Name = "name1",
                Surname = "surname1"
            };

            _uowMock.Setup(uow => uow.Users.Any(u => u.Email == newUser.Email)).Returns(true);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Register(newUser); });
            Assert.That(ex.Message, Is.EqualTo("Email currently exists."));
        }
        #endregion

        #region Login user
        [Test]
        public void LoginUser()
        {
            var userForLogin = new User
            {
                Email = "email",
                Password = "pass",
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Email.ToLower() == userForLogin.Email.Trim().ToLower(), ""))
                .Returns(fakeUser);

            _passHelperMock.Setup(pass => pass.ValidatePassword(userForLogin.Password, fakeUser.Password))
                .Returns(true);

            var resultUser = _manager.Login(userForLogin);

            Assert.IsNotNull(resultUser);
            Assert.AreEqual(fakeUser, resultUser);
        }

        [Test]
        public void LoginUserUserNotExist()
        {
            var userForLogin = new User
            {
                Email = "email",
                Password = "pass",
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Email.ToLower() == userForLogin.Email.Trim().ToLower(), ""))
                .Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Login(userForLogin); });
            Assert.That(ex.Message, Is.EqualTo("Email not exist."));

        }

        [Test]
        public void LoginUserWrongEmailOrPass()
        {
            var userForLogin = new User
            {
                Email = "email",
                Password = "pass1",
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Email.ToLower() == userForLogin.Email.Trim().ToLower(), ""))
                .Returns(fakeUser);

            _passHelperMock.Setup(pass => pass.ValidatePassword(userForLogin.Password, fakeUser.Password))
                .Returns(false);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Login(userForLogin); });
            Assert.That(ex.Message, Is.EqualTo("Wrong email or password."));

        }
        #endregion

        #region Get
        [Test]
        public void GetUser()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == 1, ""))
              .Returns(fakeUser);

            var resultUser = _manager.GetUser(1);

            Assert.IsNotNull(resultUser);
            Assert.AreEqual(fakeUser, resultUser);
        }

        [Test]
        public void GetUserNotExist()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == 1, ""))
                 .Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetUser(1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void GetUsersNoParameters()
        {
            long currentUserId = 1;

            var users = new List<User>
            {
                fakeUserFriend,
                fakeUserFriendTwo
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.Find(x => x.Id != currentUserId, "")).Returns(users.AsQueryable());

            var result = _manager.GetAllUsers(currentUserId) as List<User>;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetUsersWithParameters()
        {
            long currentUserId = 1;

            var users = new List<User>
            {
                fakeUserFriend,
                fakeUserFriendTwo
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

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);

            _propertyMappingMock.Setup(prop => prop.ValidMappingExistsFor<UserModel, User>
                (parameters.OrderBy)).Returns(true);

            _propertyMappingMock.Setup(prop => prop.GetPropertyMapping<UserModel, User>
                ()).Returns(dictionaryToReturn);

            _propertyCheckerMock.Setup(check => check.TypeHasProperties<UserModel>
                (parameters.Fields)).Returns(true);

            _uowMock.Setup(uow => uow.Users.Find(x => x.Id != currentUserId, "")).Returns(users.AsQueryable());

            var result = _manager.GetAllUsers(currentUserId, parameters) as List<User>;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        #endregion

        #region Update
        [Test]
        public void UpdateUser()
        {
            var userToUpdate = new User
            {
                Name = "name for update",
                Surname = "surname for update"
            };

            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.GetById(currentUserId)).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.Update(fakeUser, currentUserId)).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Save()).Callback(() => {
                fakeUser.Name = "name for update";
                fakeUser.Surname = "surname for update";
            });

            var resultUser = _manager.Update(currentUserId, userToUpdate);

            _uowMock.Verify(uow => uow.Users.Update(fakeUser, fakeUser.Id), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());

            Assert.IsNotNull(resultUser);
            Assert.AreEqual("name for update", resultUser.Name);
            Assert.AreEqual("surname for update", resultUser.Surname);

        }

        [Test]
        public void UpdateUserNotExist()
        {
            var userToUpdate = new User
            {
                Name = "name for update",
                Surname = "surname for update"
            };

            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.GetById(currentUserId)).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Update(currentUserId, userToUpdate); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));
        }
        #endregion

        #region Delete
        [Test]
        public void DeleteUserWithFriends()
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
            _uowMock.Setup(uow => uow.Friendships.Find(f => f.UserRecipientId == currentUserId || f.UserSenderId == currentUserId, ""))
                .Returns(friends.AsQueryable());

            _manager.DeleteUser(currentUserId);

            _uowMock.Verify(uow => uow.Friendships.Delete(It.IsAny<Friendship>()), Times.Exactly(friends.Count()));
            _uowMock.Verify(uow => uow.Users.Delete(fakeUser), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Exactly(2));

        }

        [Test]
        public void DeleteUserWithoutFriends()
        {
            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.Friendships.Find(f => f.UserRecipientId == currentUserId || f.UserSenderId == currentUserId, ""))
                .Returns((IQueryable<Friendship>)null);

            _manager.DeleteUser(currentUserId);

            _uowMock.Verify(uow => uow.Friendships.Delete(It.IsAny<Friendship>()), Times.Never);
            _uowMock.Verify(uow => uow.Users.Delete(fakeUser), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Exactly(1));
        }

        [Test]
        public void DeleteUserNotExist()
        {
            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.DeleteUser(currentUserId); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }
        #endregion

    }
}
