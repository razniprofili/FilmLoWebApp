using AutoMapper;
using Common.Helpers;
using Domain;
using Models.Friendship;
using Models.Notification;
using Models.SavedMovies;
using Models.User;
using Models.WatchedMovies;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Mapper
{
    public class Mapper
    {
        public static TDestination AutoMap<TSource, TDestination>(TSource source)
           where TDestination : class
           where TSource : class
        {
            var config = new MapperConfiguration(c => c.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        #region User mappings
        public static IEnumerable<UserModel> Map(PagedList<User> users)
        {
            List<UserModel> usersToReturn = new List<UserModel>();

            foreach (var user in users)
            {
                var userModel = new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Picture = user.Picture

                };

                usersToReturn.Add(userModel);
            }

            return usersToReturn;
        }
        #endregion

        #region Saved movies mappings
        public static IEnumerable<AddSavedMovieModel> Map(PagedList<MovieJMDBApi> movies, long currentUserId)
        {
            List<AddSavedMovieModel> moviesToReturn = new List<AddSavedMovieModel>();

            foreach (var movie in movies)
            {
                var movieModel = new AddSavedMovieModel
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Poster = movie.Poster,
                    UserId = currentUserId,
                    Actors = movie.MovieDetailsJMDBApi.Actors,
                    Genre = movie.MovieDetailsJMDBApi.Genre,
                    Duration = movie.MovieDetailsJMDBApi.Duration,
                    Year = movie.MovieDetailsJMDBApi.Year,
                    Country = movie.MovieDetailsJMDBApi.Country,
                    Director = movie.MovieDetailsJMDBApi.Director

                };

                moviesToReturn.Add(movieModel);
            }

            return moviesToReturn;
        }

        public static SavedMovieModel Map(MovieJMDBApi movie, long userId)
        {
            var userMovie = new User();
            DateTime dateTimeSaved = new DateTime();
            foreach (var movie1 in movie.SavedUsers)
            {
                if (movie1.UserId == userId)
                {
                    userMovie = movie1.User;
                    dateTimeSaved = movie1.SavingDate;
                    break;
                }

            }
            return new SavedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Poster = movie.Poster,
                DateTimeSaved = dateTimeSaved,
                UserId = userId,
                User = AutoMap<User, UserModel>(userMovie)
            };
        }

        public static AddSavedMovieModel MapAdd(MovieJMDBApi movie, long userId)
        {

            return new AddSavedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Poster = movie.Poster,
                UserId = userId,
                Actors = movie.MovieDetailsJMDBApi.Actors,
                Genre = movie.MovieDetailsJMDBApi.Genre,
                Duration = movie.MovieDetailsJMDBApi.Duration,
                Year = movie.MovieDetailsJMDBApi.Year,
                Country = movie.MovieDetailsJMDBApi.Country,
                Director = movie.MovieDetailsJMDBApi.Director
                // User = AutoMap<User, UserModel>(userMovie)
            };
        }

        #endregion

        #region Watched movies mappings
        public static IEnumerable<WatchedMovieModel> MapEnumerableWatchedMovies(PagedList<MovieJMDBApi> movies, long currentUserId)
        {
            List<WatchedMovieModel> moviesToReturn = new List<WatchedMovieModel>();

            foreach (var movie in movies)
            {
                var watchedMovie = new WatchedMovie();
                foreach (var movieIter in movie.WatchedUsers)
                {
                    if (movieIter.MovieJMDBApiId == movie.Id)
                        watchedMovie = movieIter;
                }
                var movieToadd = new WatchedMovieModel
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Actors = movie.MovieDetailsJMDBApi.Actors,
                    Year = movie.MovieDetailsJMDBApi.Year,
                    Director = movie.MovieDetailsJMDBApi.Director,
                    Duration = movie.MovieDetailsJMDBApi.Duration,
                    Genre = movie.MovieDetailsJMDBApi.Genre,
                    Country = movie.MovieDetailsJMDBApi.Country,
                    Poster = movie.Poster,
                    Rate = watchedMovie.Rating,
                    Comment = watchedMovie.Comment,
                    DateTimeWatched = watchedMovie.WatchingDate,
                    DateTimeAdded = watchedMovie.DateTimeAdded,
                    UserId = watchedMovie.UserId,
                    User = AutoMap<User, UserModel>(watchedMovie.User)
                };
                moviesToReturn.Add(movieToadd);
            }

            return moviesToReturn;

        }

        public static WatchedMovie MapUpdate(UpdateWatchedMovieModel updateModel)
        {
            return new WatchedMovie
            {
                Comment = updateModel.Comment,
                Rating = updateModel.Rate,
                WatchingDate = updateModel.DateTimeWatched
            };
        }

        public static WatchedMovieModel Map(MovieJMDBApi movie)
        {
            var watchedMovie = new WatchedMovie();
            foreach (var movieIter in movie.WatchedUsers)
            {
                if (movieIter.MovieJMDBApiId == movie.Id)
                    watchedMovie = movieIter;
            }
            return new WatchedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Actors = movie.MovieDetailsJMDBApi.Actors,
                Year = movie.MovieDetailsJMDBApi.Year,
                Director = movie.MovieDetailsJMDBApi.Director,
                Duration = movie.MovieDetailsJMDBApi.Duration,
                Genre = movie.MovieDetailsJMDBApi.Genre,
                Country = movie.MovieDetailsJMDBApi.Country,
                Poster = movie.Poster,
                Rate = watchedMovie.Rating,
                Comment = watchedMovie.Comment,
                DateTimeWatched = watchedMovie.WatchingDate,
                DateTimeAdded = watchedMovie.DateTimeAdded,
                UserId = watchedMovie.UserId,
                User = AutoMap<User, UserModel>(watchedMovie.User)
            };

        }
        public static WatchedMovieModel MapFriend(MovieJMDBApi movie, WatchedMovie watchedMovie)
        {
            return new WatchedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Actors = movie.MovieDetailsJMDBApi.Actors,
                Year = movie.MovieDetailsJMDBApi.Year,
                Director = movie.MovieDetailsJMDBApi.Director,
                Duration = movie.MovieDetailsJMDBApi.Duration,
                Genre = movie.MovieDetailsJMDBApi.Genre,
                Country = movie.MovieDetailsJMDBApi.Country,
                Poster = movie.Poster,
                Rate = watchedMovie.Rating,
                Comment = watchedMovie.Comment,
                DateTimeWatched = watchedMovie.WatchingDate,
                DateTimeAdded = watchedMovie.DateTimeAdded,
                UserId = watchedMovie.UserId,
                User = AutoMap<User, UserModel>(watchedMovie.User)
            };
        }

        public static CommentRateModel Map(WatchedMovie movie)
        {
            return new CommentRateModel
            {
                Rate = movie.Rating,
                Comment = movie.Comment,
                DateTimeWatched = movie.WatchingDate
            };
        }

        #endregion

        #region Movie mappings
        public static MovieDetailsJMDBApi MapWatchedMovie(AddWatchedMovieModel movieModel)
        {
            return new MovieDetailsJMDBApi
            {
                Name = movieModel.Name,
                Actors = movieModel.Actors,
                Year = movieModel.Year,
                Director = movieModel.Director,
                Duration = movieModel.Duration,
                Genre = movieModel.Genre,
                Country = movieModel.Country,
            };
        }

        public static MovieJMDBApi Map(AddSavedMovieModel movieModel)
        {
            return new MovieJMDBApi
            {
                Id = movieModel.Id,
                Name = movieModel.Name,
                Poster = movieModel.Poster,
                MovieDetailsJMDBApi = new MovieDetailsJMDBApi
                {
                    Actors = movieModel.Actors,
                    Genre = movieModel.Genre,
                    Duration = movieModel.Duration,
                    Year = movieModel.Year,
                    Country = movieModel.Country,
                    Name = movieModel.Name,
                    Director = movieModel.Director
                }

            };
        }
        #endregion

        #region Notification mapping
        public static NotificationModel Map(Notification notification)
        {
            return new NotificationModel
            {
                Id = notification.Id,
                Text = notification.Text,
                SendingDate = notification.SendingDate,
                UserRecipient = AutoMap<User, UserModel>(notification.UserRecipient),
                UserRecipientId = notification.UserRecipientId,
                UserSender = AutoMap<User, UserModel>(notification.UserSender),
                UserSenderId = notification.UserSenderId,

            };
        }
        #endregion

        #region Friendship mappings
        public static Friendship Map(AddFriendshipModel model, long userId)
        {
            return new Friendship
            {
                UserSenderId = userId,
                UserRecipientId = model.UserRecipientId
            };
        }

        public static FriendshipModel Map(Friendship friendship)
        {
            return new FriendshipModel
            {
                UserSenderId = friendship.UserSenderId,
                UserRecipientId = friendship.UserRecipientId,
                StatusCodeID = friendship.StatusCodeID,
                FriendshipDate = friendship.FriendshipDate,
                UserRecipient = AutoMap<User, UserModel>(friendship.UserRecipient),
                UserSender = AutoMap<User, UserModel>(friendship.UserSender)
            };
        }
        #endregion
    }
}
