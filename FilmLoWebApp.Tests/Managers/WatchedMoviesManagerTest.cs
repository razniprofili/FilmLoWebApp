using Core;
using Core.Services;
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
    public class WatchedMoviesManagerTest
    {
        #region Setup

        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyMappingService> _propertyMappingMock;
        private Mock<IPropertyCheckerService> _propertyCheckerMock;
        private IWatchedMoviesManager _manager;
        private User fakeUser;
        private User fakeUserFriend;
        private User fakeUserFriendTwo;
        private WatchedMovie fakeWatchedMoive;
        private List<User> fakeListResult;
        private List<WatchedMovie> fakeWatchedMovies;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _propertyMappingMock = new Mock<IPropertyMappingService>();
            _propertyCheckerMock = new Mock<IPropertyCheckerService>();
            _manager = new WatchedMoviesManager(_propertyMappingMock.Object, _propertyCheckerMock.Object, _uowMock.Object);
            
             fakeWatchedMoive = new WatchedMovie
            {

                UserId = 1,
                MovieJMDBApiId = "tt123",
                Comment = "comment",
                Rating = 5,
                WatchingDate = "22.02.2020.",
                DateTimeAdded = DateTime.Now,
                MovieJMDBApi = new MovieJMDBApi
                {
                    Id = "tt123",
                    MovieDetailsJMDBApi = new MovieDetailsJMDBApi
                    {

                        Actors = "Actors1",
                        Country = "SRB",
                        Director = "Director1",
                        Duration = 98,
                        Genre = "Action1",
                        Name = "Movie 123",
                        Year = 2021
                    },
                    Name = "Movie 123",
                    Poster = "poster 9",
                    SavedUsers = null,
                    WatchedUsers = null
                },
                User = fakeUser

            };
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

            fakeWatchedMovies = new List<WatchedMovie>
            {
               fakeWatchedMoive,
               new WatchedMovie
                 {
                    UserId = 1,
                    MovieJMDBApiId = "tt999",
                    DateTimeAdded = new DateTime(2020, 6, 6),
                    User = fakeUser,
                    MovieJMDBApi = new MovieJMDBApi
                    {
                        Id = "tt999",
                        MovieDetailsJMDBApi = new MovieDetailsJMDBApi
                        {

                            Actors = "Actors",
                            Country = "USA",
                            Director = "Director",
                            Duration = 111,
                            Genre = "Action",
                            Name = "Movie 99",
                            Year = 2020
                        },
                        Name = "Movie 99",
                        Poster = "poster 9",
                        SavedUsers = null,
                        WatchedUsers = null
                    },
                    WatchingDate = "12.11.2020.",
                    Rating = 4,
                    Comment = "comment"
            }
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
        public void AddMovie()
        {
            var movieToAdd = new MovieDetailsJMDBApi()
            {
                Actors = "Actors",
                Country = "USA",
                Director = "Director",
                Duration = 111,
                Genre = "Action",
                Name = "Movie 99",
                Year = 2020

            };

            var movieAPI = new MovieJMDBApi
            {
                Id = "tt123",
                MovieDetailsJMDBApi = new MovieDetailsJMDBApi
                {
                    Actors = "Actors",
                    Country = "USA",
                    Director = "Director",
                    Duration = 111,
                    Genre = "Action",
                    Name = "Movie 99",
                    Year = 2020
                },
                Name = "Movie 99",
                Poster = "poster 9",
                SavedUsers = null,
                WatchedUsers = null
            };

            long currentUserId = 1;
            string comment = "comment";
            int rate = 5;
            string date = "22.02.2020.";
            string movieId = "tt123";
            string poster = "poster";

            WatchedMovie watchedMoive = new WatchedMovie
            {

                UserId = 1,
                MovieJMDBApiId = "tt123",
                Comment = "comment",
                Rating = 5,
                WatchingDate = "22.02.2020.",
                DateTimeAdded = DateTime.Now,
                MovieJMDBApi = movieAPI,
                User = fakeUser

            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(a => a.Id == currentUserId, "")).Returns(fakeUser);

            _uowMock.Setup(uow => uow.WatchedMovies.FirstOrDefault(f => f.UserId == currentUserId && f.MovieJMDBApiId == movieId, "MovieJMDBApi"))
                .Returns((WatchedMovie)null);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(a => a.Id == movieId, "")).Returns(movieAPI);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(a => a.MovieJMDBApiId == movieId && a.UserId == currentUserId, ""))
                .Returns((SavedMovie)null);

            _uowMock.Setup(uow => uow.WatchedMovies.Add(It.Is<WatchedMovie>(m => m.UserId == 1), "")).Returns(watchedMoive);

            var result = _manager.Add(movieToAdd, currentUserId, comment, rate, date, poster, movieId);

            _uowMock.Verify(uow => uow.WatchedMovies.Add(It.Is<WatchedMovie>(m => m.UserId == 1), ""), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
            Assert.NotNull(result);
            Assert.AreEqual(movieAPI, result);
        }
        [Test]
        public void UpdateMovie()
        {
            WatchedMovie watchedMoiveForUpdate = new WatchedMovie
            {
                //user can update comment, rate OR date waching, only one, two, or all of them
                Comment = "Updated comment",
                Rating = 2
            };

            var movieAPI = new MovieJMDBApi
            {
                Id = "tt123",
                MovieDetailsJMDBApi = new MovieDetailsJMDBApi
                {
                    Actors = "Actors",
                    Country = "USA",
                    Director = "Director",
                    Duration = 111,
                    Genre = "Action",
                    Name = "Movie 99",
                    Year = 2020
                },
                Name = "Movie 99",
                Poster = "poster 9",
                SavedUsers = null,
                WatchedUsers = null
            };

            WatchedMovie watchedMoive = new WatchedMovie
            {

                UserId = 1,
                MovieJMDBApiId = "tt123",
                Comment = "comment",
                Rating = 5,
                WatchingDate = "22.02.2020.",
                DateTimeAdded = DateTime.Now,
                MovieJMDBApi = movieAPI,
                User = fakeUser

            };

            string movieId = "tt123";
            long currentUserId = 1;

            _uowMock.Setup(uow => uow.Users.GetById(currentUserId)).Returns(fakeUser);
            _uowMock.Setup(uow => uow.MoviesJMDBApi.GetById(movieId)).Returns(movieAPI);
            _uowMock.Setup(uow => uow.WatchedMovies.FirstOrDefault(f => f.MovieJMDBApiId == movieId && f.UserId == currentUserId, ""))
                .Returns(watchedMoive);

            _uowMock.Setup(uow => uow.WatchedMovies.Update(watchedMoive, currentUserId, watchedMoive.MovieJMDBApiId))
                .Returns(watchedMoive);
            _uowMock.Setup(uow => uow.Save()).Callback(() => {
                watchedMoive.Comment = "Updated comment";
                watchedMoive.Rating = 2;
            });

            var result = _manager.Update(watchedMoive, movieId, currentUserId);

            _uowMock.Verify(uow => uow.WatchedMovies.Update(watchedMoive, currentUserId, watchedMoive.MovieJMDBApiId), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());

            Assert.IsNotNull(result);

        }

        [Test]
        public void DeleteMovie()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.WatchedMovies.FirstOrDefault(p => p.UserId == 1 && p.MovieJMDBApiId == "tt123", ""))
                .Returns(fakeWatchedMoive);

            _manager.DeleteMovie("tt123", 1);

            _uowMock.Verify(uow => uow.WatchedMovies.Delete(It.IsAny<WatchedMovie>()), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
        }

        [Test]
        public void GetAllFriendMoviesNoParameters()
        {
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
            long friendId = 2;

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeUser);
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 2, ""))
                 .Returns(fakeUserFriend);
            _uowMock.Setup(uow => uow.Friendships.FirstOrDefault(f => (f.UserSenderId == currentUserId && f.UserRecipientId == friendId && f.StatusCodeID == 'A')
            || (f.UserSenderId == friendId && f.UserRecipientId == currentUserId && f.StatusCodeID == 'A'), "")).Returns(friendship);

            _uowMock.Setup(uow => uow.WatchedMovies.Find(x => x.UserId == friendId, "MovieJMDBApi"))
                 .Returns(fakeWatchedMovies.AsQueryable());

            var result = _manager.GetAllFriendMovies(currentUserId, friendId) as List<MovieJMDBApi>;

            Assert.IsNotNull(result);
            Assert.AreEqual(fakeWatchedMovies.Count(), result.Count());

        }
        #endregion

    }
}
