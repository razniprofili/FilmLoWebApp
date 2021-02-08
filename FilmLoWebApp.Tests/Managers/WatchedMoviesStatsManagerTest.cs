using Common.Exceptions;
using Core;
using Data;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmLoWebApp.Tests.Managers
{
    public class WatchedMoviesStatsManagerTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private IWatchedMoviesStatsManager _manager;
        private User fakeUser;
        private WatchedMoviesStats fakeStatsResult;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _manager = new WatchedMoviesStatsManager(_uowMock.Object);
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
            fakeStatsResult = new WatchedMoviesStats
            {
                UserId = 1, 
                AverageRate = 5, 
                TotalCount = 10, 
                TotalTime = 60
            };
        }

        [Test]
        public void GetWatchedMoviesStats()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeUser);
            _uowMock.Setup(uow => uow.WatchedMoviesStats.FirstOrDefault(stat => stat.UserId == 1, ""))
                .Returns(fakeStatsResult);

            var result = _manager.GetWatchedMoviesStats(fakeUser.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(fakeStatsResult, result);
        }

        [Test]

        public void GetWatchedMoviesStatsUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetWatchedMoviesStats(1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }
    }
}
