using Common.Exceptions;
using Common.ResourceParameters;
using Core;
using Core.Services;
using Data;
using Domain;
using Models.SavedMovies;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmLoWebApp.Tests.Managers
{
    public class SavedMoviesManagerTest
    {

        #region Setup

        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyMappingService> _propertyMappingMock;
        private Mock<IPropertyCheckerService> _propertyCheckerMock;
        private ISavedMoviesManager _manager;
        private User fakeUser;
        private SavedMovie fakeSavedMovie;
        private MovieJMDBApi fakeMovieJMDBApi;
        private List<SavedMovie> fakeListResult;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _propertyMappingMock = new Mock<IPropertyMappingService>();
            _propertyCheckerMock = new Mock<IPropertyCheckerService>();

            _manager = new SavedMoviesManager(_propertyMappingMock.Object, _propertyCheckerMock.Object, _uowMock.Object);

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

            fakeMovieJMDBApi = new MovieJMDBApi
            {
                Id = "tt123",
                MovieDetailsJMDBApi = new MovieDetailsJMDBApi
                {

                    Actors = "actors",
                    Country = "SRB",
                    Director = "Director",
                    Duration = 111,
                    Genre = "comedy",
                    Name = "Movie 1",
                    Year = 2021
                },
                Name = "Movie 1",
                Poster = "poster 1",
                SavedUsers = null,
                WatchedUsers = null
            };

            fakeSavedMovie = new SavedMovie
            {
                UserId = 1,
                User = fakeUser,
                MovieJMDBApiId = "tt123",
                MovieJMDBApi = fakeMovieJMDBApi,
                SavingDate = new DateTime(2020, 5, 5)
            };

            fakeListResult = new List<SavedMovie>
            {
               fakeSavedMovie,
               new SavedMovie
                 {
                    UserId = 1,
                    MovieJMDBApiId = "tt999",
                    SavingDate = new DateTime(2020, 6, 6),
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
                    }
            }
        };
        }
        #endregion

        #region Delete saved movie
        [Test]
        public void DeleteSavedMovie()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                .Returns(fakeUser);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(p => p.UserId == 1 && p.MovieJMDBApiId == "tt123", ""))
                .Returns(fakeSavedMovie);

            _manager.Delete(1, "tt123");

            _uowMock.Verify(uow => uow.SavedMovies.Delete(It.IsAny<SavedMovie>()), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());
        }

        [Test]
        public void DeleteSavedMovieUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Delete(1, "tt123"); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void DeleteSavedMovieMovieNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(p => p.UserId == 1 && p.MovieJMDBApiId == "tt123", ""))
                .Returns((SavedMovie)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Delete(1, "tt123"); });
            Assert.That(ex.Message, Is.EqualTo("SavedMovie not exist!"));

        }
        #endregion

        #region Get saved movie
        [Test]
        public void GetSavedMovie()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                  .Returns(fakeUser);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(m => m.Id == "tt123", ""))
                .Returns(fakeMovieJMDBApi);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(p => p.UserId == 1 && p.MovieJMDBApiId == "tt123", "MovieJMDBApi"))
                .Returns(fakeSavedMovie);

            var result = _manager.GetMovie(fakeUser.Id, "tt123");

            Assert.IsNotNull(result);
            Assert.AreEqual(fakeMovieJMDBApi, result);
        }

        [Test]
        public void GetSavedMovieUserNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, "")).Returns((User)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetMovie(1, "tt123"); });
            Assert.That(ex.Message, Is.EqualTo("User not exist!"));

        }

        [Test]
        public void GetSavedMovieMovieJMDBApiNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(m => m.Id == "tt123", ""))
                .Returns((MovieJMDBApi)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetMovie(1, "tt123"); });
            Assert.That(ex.Message, Is.EqualTo("MovieJMDBApi not exist!"));

        }

        [Test]
        public void GetSavedMovieSavedMovieNotExists()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(m => m.Id == "tt123", ""))
                .Returns(fakeMovieJMDBApi);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(p => p.UserId == 1 && p.MovieJMDBApiId == "tt123", "MovieJMDBApi"))
                .Returns((SavedMovie)null);

            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.GetMovie(1, "tt123"); });
            Assert.That(ex.Message, Is.EqualTo("SavedMovie not exist!"));

        }
        #endregion

        #region Add saved movie
        [Test]
        public void AddSavedMovieMovieJMDBApiExixstInDb()
        {
            var movieToAdd = new MovieJMDBApi
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
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(m => m.Id == movieToAdd.Id, ""))
                 .Returns(movieToAdd);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(m => m.UserId == 1 && m.MovieJMDBApiId == "tt999", ""))
                 .Returns((SavedMovie)null);

            SavedMovie savedMovie = new SavedMovie
            {
                UserId = 1,
                MovieJMDBApiId = movieToAdd.Id,
                SavingDate = DateTime.Now
            };

            SavedMovie addedSavedMovie = new SavedMovie
            {
                UserId = 1,
                MovieJMDBApiId = "tt999",
                SavingDate = DateTime.Now,
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
                    SavedUsers = new List<SavedMovie> {
                        new SavedMovie {
                            MovieJMDBApi = movieToAdd,
                            MovieJMDBApiId = "tt999",
                            SavingDate = DateTime.Now,
                            User = fakeUser,
                            UserId = 1
                        }
                    },
                    WatchedUsers = null
                }

            };

            _uowMock.Setup(uow => uow.SavedMovies.Add(savedMovie, ""))
                  .Returns(addedSavedMovie);

            var result = _manager.Add(1, movieToAdd);

            // _uowMock.Verify(uow => uow.SavedMovies.Add(savedMovie, ""), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Once());

            Assert.AreEqual(movieToAdd.Id, result.Id);
            Assert.AreEqual(movieToAdd.MovieDetailsJMDBApi, result.MovieDetailsJMDBApi);
        }

        [Test]
        public void AddSavedMovieMovieJMDBApiNotExixstInDb()
        {
            var movieToAdd = new MovieJMDBApi
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
            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(m => m.Id == movieToAdd.Id, ""))
                 .Returns((MovieJMDBApi)null);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.Add(movieToAdd, ""))
                 .Returns(movieToAdd);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(m => m.UserId == 1 && m.MovieJMDBApiId == "tt999", ""))
                 .Returns((SavedMovie)null);

            SavedMovie savedMovie = new SavedMovie
            {
                UserId = 1,
                MovieJMDBApiId = movieToAdd.Id,
                SavingDate = DateTime.Now
            };

            SavedMovie addedSavedMovie = new SavedMovie
            {
                UserId = 1,
                MovieJMDBApiId = "tt999",
                SavingDate = DateTime.Now,
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
                    SavedUsers = new List<SavedMovie> {
                        new SavedMovie {
                            MovieJMDBApi = movieToAdd,
                            MovieJMDBApiId = "tt999",
                            SavingDate = DateTime.Now,
                            User = fakeUser,
                            UserId = 1
                        }
                    },
                    WatchedUsers = null
                }

            };

            _uowMock.Setup(uow => uow.SavedMovies.Add(savedMovie, ""))
                  .Returns(addedSavedMovie);

            var result = _manager.Add(1, movieToAdd);

            // _uowMock.Verify(uow => uow.SavedMovies.Add(savedMovie, ""), Times.Once());
            _uowMock.Verify(uow => uow.MoviesJMDBApi.Add(movieToAdd, ""), Times.Once());
            _uowMock.Verify(uow => uow.Save(), Times.Exactly(2));

            Assert.AreEqual(movieToAdd.Id, result.Id);
            Assert.AreEqual(movieToAdd.MovieDetailsJMDBApi, result.MovieDetailsJMDBApi);
        }

        [Test]
        public void AddSavedMovieSavedMovieExist()
        {
            var movieToAdd = new MovieJMDBApi
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
            };

            SavedMovie savedMovie = new SavedMovie
            {
                UserId = 1,
                MovieJMDBApiId = "tt999",
                SavingDate = DateTime.Now,
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
                    SavedUsers = new List<SavedMovie> {
                        new SavedMovie {
                            MovieJMDBApi = movieToAdd,
                            MovieJMDBApiId = "tt999",
                            SavingDate = DateTime.Now,
                            User = fakeUser,
                            UserId = 1
                        }
                    },
                    WatchedUsers = null
                }

            };

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.MoviesJMDBApi.FirstOrDefault(m => m.Id == movieToAdd.Id, ""))
                 .Returns(movieToAdd);

            _uowMock.Setup(uow => uow.SavedMovies.FirstOrDefault(m => m.UserId == 1 && m.MovieJMDBApiId == movieToAdd.Id, ""))
                 .Returns(savedMovie);


            Exception ex = Assert.Throws<ValidationException>(delegate { _manager.Add(1, movieToAdd); });
            Assert.That(ex.Message, Is.EqualTo("SavedMovie currently exists!"));

        }
        #endregion

        #region Get all saved movies
        [Test]
        public void GetSavedMoviesNoParameters()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.SavedMovies.Find(m => m.UserId == 1, "MovieJMDBApi"))
                .Returns(fakeListResult.AsQueryable());

            var resultList = _manager.GetAllMovies(1) as List<MovieJMDBApi>;

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.ToList().Count());

        }

        [Test]
        public void GetSavedMoviesNoParametersNoSavedMovies()
        {
            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _uowMock.Setup(uow => uow.SavedMovies.Find(m => m.UserId == 1, "MovieJMDBApi"))
                .Returns(new List<SavedMovie>().AsQueryable());

            var resultList = _manager.GetAllMovies(1) as List<MovieJMDBApi>;

            Assert.IsNotNull(resultList);
            Assert.AreEqual(0, resultList.ToList().Count());

        }

        [Test]
        public void GetSavedMoviesWithParameters()
        {
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

            _uowMock.Setup(uow => uow.Users.FirstOrDefault(x => x.Id == 1, ""))
                 .Returns(fakeUser);

            _propertyMappingMock.Setup(prop => prop.ValidMappingExistsFor<AddSavedMovieModel, MovieJMDBApi>
                (parameters.OrderBy)).Returns(true);

            _propertyMappingMock.Setup(prop => prop.GetPropertyMapping<AddSavedMovieModel, MovieJMDBApi>
                ()).Returns(dictionaryToReturn);


            _propertyCheckerMock.Setup(check => check.TypeHasProperties<AddSavedMovieModel>
                (parameters.Fields)).Returns(true);

            _uowMock.Setup(uow => uow.SavedMovies.Find(m => m.UserId == 1, "MovieJMDBApi"))
                .Returns(fakeListResult.AsQueryable());


            var resultList = _manager.GetAllMovies(1, parameters) as List<MovieJMDBApi>;

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.ToList().Count());

        }


        #endregion

    }
}
