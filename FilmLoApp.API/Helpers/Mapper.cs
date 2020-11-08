using AutoMapper;
using Domain;
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

        public static SavedMovieModel Map(MovieJMDBApi movie, long userId)
        {
            var userMovie = new User();
            foreach(var movie1 in movie.Users)
            {
                if( movie1.UserId == userId)
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

        public static WatchedMovieModel Map(MovieDetailsJMDBApi movie, long userId)
        {
            var watchedMovie = new WatchedMovie();
            foreach (var movie1 in movie.Users)
            {
                if (movie1.UserId == userId && movie1.MovieDetailsJMDBApiId == movie.Id)
                {
                    watchedMovie = movie1;
                    break;
                }

            }
            return new WatchedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Actors = movie.Actors,
                Year = movie.Year,
                Director = movie.Director,
                Duration = movie.Duration,
                Genre = movie.Genre,
                Country = movie.Country,
                Rate = watchedMovie.Rating,
                Comment = watchedMovie.Comment,
                DateTimeWatched = watchedMovie.WatchingDate

            };
        }

        public static WatchedMovieModel Map(MovieDetailsJMDBApi movie)
        {
          
            return new WatchedMovieModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Actors = movie.Actors,
                Year = movie.Year,
                Director = movie.Director,
                Duration = movie.Duration,
                Genre = movie.Genre,
                Country = movie.Country,
                //Rate = watchedMovie.Rating,
                //Comment = watchedMovie.Comment,
                //DateTimeWatched = watchedMovie.WatchingDate

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

        //public static WatchedMovieModel MapGet(MovieDetailsJMDBApi movie, long userID)
        //{
        //    var 
        //    return new WatchedMovieModel
        //    {
        //        Id = movie.Id,
        //        Name = movie.Name,
        //        Actors = movie.Actors,
        //        Year = movie.Year,
        //        Director = movie.Director,
        //        Duration = movie.Duration,
        //        Genre = movie.Genre,
        //        Country = movie.Country,
        //        //Rate = watchedMovie.Rating,
        //        //Comment = watchedMovie.Comment,
        //        //DateTimeWatched = watchedMovie.WatchingDate

        //    };
        //}
    }
}
