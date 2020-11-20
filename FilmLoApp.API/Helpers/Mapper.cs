using AutoMapper;
using Core;
using Data.Repositories;
using Domain;
using FilmLoApp.API.Models.Friendship;
using FilmLoApp.API.Models.SavedMovies;
using FilmLoApp.API.Models.User;
using FilmLoApp.API.Models.WatchedMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Helpers
{
    public static class Mapper
    {
        //mapira npr Usera u UserModel, to se pise u <>, znaci User je ulazni parametar, a povr je UserModel
        public static TDestination AutoMap<TSource, TDestination>(TSource source)
            where TDestination : class
            where TSource : class
        {
            var config = new MapperConfiguration(c => c.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        // ovde dodati jos mapera po potrebi

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
            foreach(var movieIter in movie.WatchedUsers)
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
