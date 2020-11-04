using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public List<MovieDetailsJMDBApi> GetAllFriendMovies(long userId, long friendId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var userFriend = uow.UserRepository.FirstOrDefault(a => a.Id == friendId);
                ValidationHelper.ValidateNotNull(userFriend);

                //provera prijateljstva:
                var exist = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == userId && f.UserRecipientId == friendId && f.StatusCodeID == 'A');
                ValidationHelper.ValidateEntityExists(exist);

                var watchedMovies = uow.WatchedMovieRepository.Find(m => m.UserId == friendId); //pronalazi sve sacuvane filmove za tog prijatelja

                List<MovieDetailsJMDBApi> usersWatchedMovies = new List<MovieDetailsJMDBApi>();

                foreach (var movie in watchedMovies) // za sve te sacuvane filmove uzima njihove detalje
                {
                    var movieAPI = uow.MovieDetailsJMDBApiRepository.GetById(movie.MovieDetailsJMDBApiId);
                    usersWatchedMovies.Add(movieAPI);
                }

                return usersWatchedMovies;
            }
        }

        // odgledani filmovi svih mojih prijatelja:

        public List<MovieDetailsJMDBApi> GetAllFriendsMovies(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                //pretraga prijateljstva:
                var friendships = uow.FriendshipRepository.Find(f => f.UserSenderId == userId && f.StatusCodeID == 'A');
                ValidationHelper.ValidateNotNull(friendships);

                List<MovieDetailsJMDBApi> friendsWatchedMovies = new List<MovieDetailsJMDBApi>();

                foreach ( var friend in friendships)
                {
                    var watchedMovies = uow.WatchedMovieRepository.Find(m => m.UserId == friend.UserRecipientId); //pronalazi sve sacuvane filmove za tog prijatelja

                    foreach (var movie in watchedMovies) // za sve te sacuvane filmove uzima njihove detalje
                    {
                        var movieAPI = uow.MovieDetailsJMDBApiRepository.GetById(movie.MovieDetailsJMDBApiId);
                        friendsWatchedMovies.Add(movieAPI);
                    }
                }

                return friendsWatchedMovies;
            }
        }

        //prijatelji koji su takodje odgledali taj film:

        public List<User> FriendsWatchThatMovie(long userId, string movieName)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var friendships = uow.FriendshipRepository.Find(f => f.UserSenderId == userId && f.StatusCodeID == 'A');

                var friends = new List<User>(); // useri koji su mi prijatelji
                var friendsWatched = new List<User>();
                var friendsWatchedMovies = new List<MovieDetailsJMDBApi>();

                foreach(var friend in friendships)
                {
                    var friendAdd = uow.UserRepository.FirstOrDefault(a => a.Id == friend.UserRecipientId);
                    friends.Add(friendAdd);
                }

                foreach( var friend in friends)
                {
                    friendsWatchedMovies = GetAllFriendsMovies(friend.Id); // za svakog usera prijatelja uzimamo sve njegove odgledane filmove

                    foreach(var movie in friendsWatchedMovies)
                    {
                        if (movie.Name.ToLower() == movieName.ToLower()) { // ukoliko je neki prijatelj odgledao taj film dodajemo ga u listu
                            friendsWatched.Add(friend);
                            break;
                        }      
                    }
                }

                return friendsWatched;
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

        public WatchedMovie GetFriendCommentRate(long movieId, long userId, long friendId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var userFriend = uow.UserRepository.FirstOrDefault(a => a.Id == friendId);
                ValidationHelper.ValidateNotNull(userFriend);

                //provera prijateljstva:
                var exist = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == userId && f.UserRecipientId == friendId && f.StatusCodeID == 'A');
                ValidationHelper.ValidateEntityExists(exist);

                var movie = uow.WatchedMovieRepository.FirstOrDefault(a => a.UserId == friendId && a.MovieDetailsJMDBApiId == movieId);
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

        //public List<MovieDetailsJMDBApi> GetAllFriendMovies(long userId, long friendId)
        //{
        //    using (var uow = new UnitOfWork())
        //    {
        //        //provera da li postoji useri i prijateljstvo za svaki slucaj:
        //        var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
        //        ValidationHelper.ValidateNotNull(user);

        //        var userFriend = uow.UserRepository.FirstOrDefault(a => a.Id == friendId);
        //        ValidationHelper.ValidateNotNull(user);

        //        var friendship = uow.FriendshipRepository.FirstOrDefault(f => f.UserSenderId == userId && f.UserRecipientId == friendId);
        //        ValidationHelper.ValidateNotNull(friendship);

        //        var watchedMovies = uow.WatchedMovieRepository.Find(m => m.UserId == friendId); //pronalazi sve odgledane filmove za tog prijatelja

        //        List<MovieDetailsJMDBApi> usersWatchedMovies = new List<MovieDetailsJMDBApi>();

        //        foreach (var movie in watchedMovies)
        //        {
        //            var movieAPI = uow.MovieDetailsJMDBApiRepository.GetById(movie.MovieDetailsJMDBApiId);
        //            usersWatchedMovies.Add(movieAPI);
        //        }

        //        return usersWatchedMovies;
        //    }
        //}

        public long CountMovies(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.WatchedMovieRepository.Count(a => a.UserId == userId);
            }
        }


        public MovieDetailsJMDBApi Add(MovieDetailsJMDBApi movie, long userId,string comment, int rate, string date)
        {
            using (var uow = new UnitOfWork())
            {
                // user mora da postoji
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                // film mora postojati u bazi da bi se dodala asocijativna klasa, ako ne postoji dodajemo ga
                // film postoji ako vec ima taj naziv, poredimo mala slova imena

                var movieDetailsExist = uow.MovieDetailsJMDBApiRepository.FirstOrDefault(a => a.Name.ToLower() == movie.Name.ToLower());

                if (movieDetailsExist != null)
                {
                    //provera da li je vec unet film za tog usera:
                    var exist = uow.WatchedMovieRepository.FirstOrDefault(f => f.UserId == userId && f.MovieDetailsJMDBApiId == movieDetailsExist.Id);
                    ValidationHelper.ValidateEntityExists(exist);

                    WatchedMovie watchedMoive = new WatchedMovie
                    {
                        UserId = userId,
                        MovieDetailsJMDBApiId = movieDetailsExist.Id,
                        Comment = comment,
                        Rating = rate,
                        WatchingDate = date
                    };

                    
                    uow.WatchedMovieRepository.Add(watchedMoive);
                    uow.Save();

                    return movieDetailsExist;

                } else
                {
                    //moramo da ga dodamo a nakon toga i asocijativnu klasu
                    var newMovie =  uow.MovieDetailsJMDBApiRepository.Add(movie); // dobijamo genericki id, PAZI
                    uow.Save();

                    //provera da li je vec unet film za tog usera:
                    var exist = uow.WatchedMovieRepository.FirstOrDefault(f => f.UserId == userId && f.MovieDetailsJMDBApiId == newMovie.Id);
                    ValidationHelper.ValidateEntityExists(exist);

                    // ako nije dodajemo ga
                    WatchedMovie watchedMoive = new WatchedMovie
                    {
                        UserId = userId,
                        MovieDetailsJMDBApiId = newMovie.Id,
                        Comment = comment,
                        Rating = rate,
                        WatchingDate = date
                    };

                    uow.WatchedMovieRepository.Add(watchedMoive);
                    uow.Save();

                    return newMovie;
                }
                //return movie;
           
            }
        }

        // update movie

        public MovieDetailsJMDBApi Update(MovieDetailsJMDBApi movie, string comment, string rate, long userId)
        {
            using (var uow = new UnitOfWork())
            {
                var userExists = uow.UserRepository.GetById(userId);
                ValidationHelper.ValidateNotNull(userExists);

                var movieExists = uow.MovieDetailsJMDBApiRepository.GetById(movie.Id);
                ValidationHelper.ValidateNotNull(movieExists);

                // da li je taj user pogledao taj film:
                var watchedMovie = uow.WatchedMovieRepository.FirstOrDefault(m => m.MovieDetailsJMDBApiId == movie.Id && m.UserId == userId);
                ValidationHelper.ValidateNotNull(watchedMovie);

                if (movie.Name == null)
                    movie.Name = movieExists.Name;

                if (movie.Year == null)
                    movie.Year = movieExists.Year;

                if (movie.Duration == null)
                    movie.Duration = movieExists.Duration;

                if (movie.Director == null)
                    movie.Director = movieExists.Director;

                if (movie.Country == null)
                    movie.Country = movieExists.Country;

                if (movie.Actors == null)
                    movie.Actors = movieExists.Actors;

                if (movie.Genre == null)
                    movie.Genre = movieExists.Genre;


                uow.MovieDetailsJMDBApiRepository.Update(movie, movie.Id);
                uow.Save();

              //  WatchedMovie updatedMovie = new WatchedMovie();

                if (comment != null)
                    watchedMovie.Comment = comment;

                if (rate != null)
                    watchedMovie.Rating = Int32.Parse(rate);

                uow.WatchedMovieRepository.Update(watchedMovie, watchedMovie.MovieDetailsJMDBApiId);
                uow.Save();


                return movie;

            }
        }
    }
}
