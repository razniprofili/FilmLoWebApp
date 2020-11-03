using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core
{
    public class WatchedMoviesManager
    {
        public List<MovieDetailsJMDBApi> GetAllMovies(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var watchedMovies = uow.WatchedMovieRepository.Find(m => m.UserId == userId); //pronalazi sve sacuvane filmove za tog usera

                List<MovieDetailsJMDBApi> usersWatchedMovies = new List<MovieDetailsJMDBApi>();

                foreach (var movie in watchedMovies) // za sve te sacuvane filmove uzima njihove detalje
                {
                    var movieAPI = uow.MovieDetailsJMDBApiRepository.GetById(movie.MovieDetailsJMDBApiId);
                    usersWatchedMovies.Add(movieAPI);
                }

                return usersWatchedMovies;
            }
        }

        public WatchedMovie GetCommentRate(long movieId, long userId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var movie = uow.WatchedMovieRepository.FirstOrDefault(a => a.UserId == userId && a.MovieDetailsJMDBApiId == movieId);
                ValidationHelper.ValidateNotNull(movie);

                return movie;
            }
        }

        public MovieDetailsJMDBApi GetMovie(long movieId, long userId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var watchedMovie = uow.WatchedMovieRepository.FirstOrDefault(a => a.UserId == userId && a.MovieDetailsJMDBApiId == movieId);
                ValidationHelper.ValidateNotNull(watchedMovie);

                var movie = uow.MovieDetailsJMDBApiRepository.GetById(movieId);
                ValidationHelper.ValidateNotNull(movie);

                return movie;
            }
        }

        public void DeleteMovie(long movieId, long userId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var watchedMovieDelete = uow.WatchedMovieRepository.FirstOrDefault(a => a.UserId == userId && a.MovieDetailsJMDBApiId == movieId);
                ValidationHelper.ValidateNotNull(watchedMovieDelete);

                uow.WatchedMovieRepository.Delete(watchedMovieDelete);
                uow.Save();
            }
        }

        public List<MovieDetailsJMDBApi> GetAllFriendMovies(long userId, long friendId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji useri i prijateljstvo za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var userFriend = uow.UserRepository.FirstOrDefault(a => a.Id == friendId);
                ValidationHelper.ValidateNotNull(user);

                var friendship = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == userId && f.UserRecipientId == friendId);
                ValidationHelper.ValidateNotNull(friendship);

                var watchedMovies = uow.WatchedMovieRepository.Find(m => m.UserId == friendId); //pronalazi sve odgledane filmove za tog prijatelja

                List<MovieDetailsJMDBApi> usersWatchedMovies = new List<MovieDetailsJMDBApi>();

                foreach (var movie in watchedMovies)
                {
                    var movieAPI = uow.MovieDetailsJMDBApiRepository.GetById(movie.MovieDetailsJMDBApiId);
                    usersWatchedMovies.Add(movieAPI);
                }

                return usersWatchedMovies;
            }
        }

        public long CountMovies(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.WatchedMovieRepository.Count(a => a.UserId == userId);
            }
        }



        // Add Movie Method
    }
}
