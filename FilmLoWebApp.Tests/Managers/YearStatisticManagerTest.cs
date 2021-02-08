using Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Domain;
using System.Linq;
using Common.Exceptions;

namespace FilmLoWebApp.Tests.Managers
{
   public class YearStatisticManagerTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private IYearStatisticManager _manager;
        private User fakeUser;
        private List<YearStatistic> fakeListResult;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _manager = new YearStatisticManager(_uowMock.Object);
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
            fakeListResult = new List<YearStatistic>
            {
                new YearStatistic {UserId = 1, Count= 11, Year= "2020."},
                new YearStatistic {UserId = 1, Count= 5, Year= "2021."},
            };
        }

        [Test]
        public void GetYearStatistic()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.YearStatistic.Find(stat => stat.UserId == 1, "")).Returns(fakeListResult.AsQueryable());

            var resultList = _manager.GetYearStatistic(fakeUser.Id);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.ToList().Count());
        }

        [Test]

        public void GetYearStatisticUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);
            
            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetYearStatistic(1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));          

        }
    }
}
