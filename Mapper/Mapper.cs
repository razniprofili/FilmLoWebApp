using AutoMapper;
using Domain;
using Models.Friendship;
using Models.SavedMovies;
using Models.User;
using Models.WatchedMovies;
using System;

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
                Rate = watchedMovie.Rating,
                Comment = watchedMovie.Comment,
                DateTimeWatched = watchedMovie.WatchingDate,
                UserId = watchedMovie.UserId,
                User = AutoMap<User, UserModel>(watchedMovie.User)
            };

        }

        public static SavedMovieModel Map(MovieJMDBApi movie, long userId)
        {
            var userMovie = new User();
            foreach (var movie1 in movie.SavedUsers)
            {
                if (movie1.UserId == userId)
                {
                    userMovie = movie1.User;
                    break;
                }

            }
            return new SavedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Poster = movie.Poster,
                UserId = userId,
                User = AutoMap<User, UserModel>(userMovie)
            };
        }

        public static Friendship Map(AddFriendshipModel model, long userId)
        {
            return new Friendship
            {
                UserSenderId = userId,
                UserRecipientId = model.UserRecipientId
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
                Rate = watchedMovie.Rating,
                Comment = watchedMovie.Comment,
                DateTimeWatched = watchedMovie.WatchingDate,
                UserId = watchedMovie.UserId,
                User = AutoMap<User, UserModel>(watchedMovie.User)
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

        public static CommentRateModel Map(WatchedMovie movie)
        {
            return new CommentRateModel
            {
                Rate = movie.Rating,
                Comment = movie.Comment,
                DateTimeWatched = movie.WatchingDate
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
    }
}
