using Common.Exceptions;
using Core;
using Data;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmLoWebApp.Tests.Managers
{
    public class PopularMoviesManagerTest
    {
        #region Setup
        private Mock<IUnitOfWork> _uowMock;
        private IPopularMoviesManager _manager;
        private User fakeUser;
        private List<PopularMovies> fakeListResult;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _manager = new PopularMoviesManager(_uowMock.Object);
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
            fakeListResult = new List<PopularMovies>
            {
                new PopularMovies {UserId = 1, MovieId = "tt1234", MovieName = "Movie 1"},
                new PopularMovies {UserId = 1, MovieId = "tt12345", MovieName = "Movie 2"},
            };
        }
        #endregion

        #region Tests
        [Test]
        public void GetPopularMovies()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns(fakeUser);
            _uowMock.Setup(uow => uow.PopularMovies.Find(stat => stat.UserId == 1, "")).Returns(fakeListResult.AsQueryable());

            var resultList = _manager.GetPopularMovies(fakeUser.Id);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.ToList().Count());
        }

        [Test]

        public void GetPopularMoviesUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetPopularMovies(1); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }
        #endregion

    }
}
