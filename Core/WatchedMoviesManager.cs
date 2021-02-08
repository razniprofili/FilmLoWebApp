using Common.Exceptions;
using Common.Helpers;
using Common.ResourceParameters;
using Core.Services;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.Data.SqlClient.Server;
using Models.WatchedMovies;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core
{
    public class WatchedMoviesManager : IWatchedMoviesManager
    {

        #region Fields

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _servicePropertyChecker;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Constructor

        public WatchedMoviesManager(IPropertyMappingService propertyMappingService, IPropertyCheckerService checker,
            IUnitOfWork uow)
        {
            _servicePropertyChecker = checker;
            _propertyMappingService = propertyMappingService;
            _uow = uow;
        }

        #endregion

        #region Methods

        public object GetAllMovies(long userId, ResourceParameters parameters = null)
        {

            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            if( parameters != null)
            {
                // provera da li postoje polja za sort
                if (!_propertyMappingService.ValidMappingExistsFor<WatchedMovieModel, MovieJMDBApi>
                (parameters.OrderBy))
                {
                    throw new ValidationException($"{parameters.OrderBy} fields for ordering do not exist!");
                }

                //provera da li postoji properti za data shaping
                if (!_servicePropertyChecker.TypeHasProperties<WatchedMovieModel>
                    (parameters.Fields))
                {
                    throw new ValidationException($"{parameters.Fields} fields for shaping do not exist!");
                }
            }

            var watchedMovies = _uow.WatchedMovies.Find(m => m.UserId == userId, "MovieJMDBApi").ToList(); //pronalazi sve odgledane filmove za tog usera

            List<MovieJMDBApi> usersWatchedMovies = new List<MovieJMDBApi>();

            foreach (var movie in watchedMovies) // za sve te odgledane filmove uzima njihove detalje
            {
                usersWatchedMovies.Add(movie.MovieJMDBApi);
            }

            if (parameters != null)
                return generateResult(usersWatchedMovies, parameters);
            else
                return usersWatchedMovies;
        }

        // with hateoas, fields, orderBy, search, pagination ...
        public object GetAllFriendMovies(long userId, long friendId, ResourceParameters parameters = null)
        {

            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var userFriend = _uow.Users.FirstOrDefault(a => a.Id == friendId, "");
            ValidationHelper.ValidateNotNull(userFriend);

            //provera prijateljstva:
            var exist = _uow.Friendships.FirstOrDefault(f => (f.UserSenderId == userId && f.UserRecipientId == friendId && f.StatusCodeID == 'A') 
            || (f.UserSenderId == friendId && f.UserRecipientId == userId && f.StatusCodeID == 'A'), "");
            ValidationHelper.ValidateNotNull(exist);

            if(parameters != null)
            {
                // provera da li postoje polja za sort
                if (!_propertyMappingService.ValidMappingExistsFor<WatchedMovieModel, MovieJMDBApi>
                (parameters.OrderBy))
                {
                    throw new ValidationException($"{parameters.OrderBy} fields for ordering do not exist!");
                }

                //provera da li postoji properti za data shaping
                if (!_servicePropertyChecker.TypeHasProperties<WatchedMovieModel>
                    (parameters.Fields))
                {
                    throw new ValidationException($"{parameters.Fields} fields for shaping do not exist!");
                }
            }
                
            var watchedMovies = _uow.WatchedMovies.Find(m => m.UserId == friendId, "MovieJMDBApi").ToList(); //pronalazi sve odgledane filmove za tog prijatelja

            List<MovieJMDBApi> usersWatchedMovies = new List<MovieJMDBApi>();

            foreach (var movie in watchedMovies) // za sve te sacuvane filmove uzima njihove detalje
            {
                usersWatchedMovies.Add(movie.MovieJMDBApi);
            }

            if (parameters != null)
                return generateResult(usersWatchedMovies, parameters);
            else
                return usersWatchedMovies;
        }

        public object GetAllFriendsMovies(long userId, ResourceParameters parameters = null)
        {

            //provera da li postoji user za svaki slucaj:
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            //pretraga prijateljstva:
            var friendships = _uow.Friendships.Find(f => (f.UserSenderId == userId && f.StatusCodeID == 'A') || (f.UserRecipientId == userId && f.StatusCodeID == 'A')).ToList();
            ValidationHelper.ValidateNotNull(friendships);

            if( parameters != null)
            {
                // provera da li postoje polja za sort
                if (!_propertyMappingService.ValidMappingExistsFor<WatchedMovieModel, MovieJMDBApi>
                (parameters.OrderBy))
                {
                    throw new ValidationException($"{parameters.OrderBy} fields for ordering do not exist!");
                }

                //provera da li postoji properti za data shaping
                if (!_servicePropertyChecker.TypeHasProperties<WatchedMovieModel>
                    (parameters.Fields))
                {
                    throw new ValidationException($"{parameters.Fields} fields for shaping do not exist!");
                }
            }

            List<MovieJMDBApi> friendsWatchedMovies = new List<MovieJMDBApi>();

            foreach (var friend in friendships)
            {
                if (friend.UserRecipientId == userId)
                {
                    var watchedMovies = _uow.WatchedMovies.Find(m => m.UserId == friend.UserSenderId, "User").ToList(); //pronalazi sve sacuvane filmove za tog prijatelja

                    foreach (var movie in watchedMovies) // za sve te sacuvane filmove uzima njihove detalje
                    {
                        var movieAPI = _uow.MoviesJMDBApi.GetById(movie.MovieJMDBApiId);
                        friendsWatchedMovies.Add(movieAPI);
                    }
                }
                else
                {
                    var watchedMovies = _uow.WatchedMovies.Find(m => m.UserId == friend.UserRecipientId, "User").ToList(); //pronalazi sve sacuvane filmove za tog prijatelja

                    foreach (var movie in watchedMovies) // za sve te sacuvane filmove uzima njihove detalje
                    {
                        var movieAPI = _uow.MoviesJMDBApi.GetById(movie.MovieJMDBApiId);
                        friendsWatchedMovies.Add(movieAPI);
                    }
                }
            }
            if (parameters != null)
                return generateResult(friendsWatchedMovies, parameters);
            else
                return friendsWatchedMovies;

        }

        //prijatelji koji su takodje odgledali taj film:

        public List<User> FriendsWatchThatMovie(long userId, string movieId)
        {

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            var friendships = _uow.Friendships.Find(f => (f.UserSenderId == userId && f.StatusCodeID == 'A') 
            || (f.UserRecipientId == userId && f.StatusCodeID == 'A')).ToList();

            var friends = new List<User>(); // useri koji su mi prijatelji
            var friendsWatched = new List<User>();
            var friendsWatchedMovies = new List<MovieJMDBApi>();

            foreach (var friend in friendships)
            {
                if (friend.UserRecipientId == userId) // ako sam ja primla zahtev za prijateljstvo, moji prijatelji su oni koji su mi poslali zahtev
                {
                    var friendAdd = _uow.Users.FirstOrDefault(a => a.Id == friend.UserSenderId);
                    friends.Add(friendAdd);

                }
                else // obrnuta situacija
                {
                    var friendAdd = _uow.Users.FirstOrDefault(a => a.Id == friend.UserRecipientId);
                    friends.Add(friendAdd);
                }
            }

            foreach (var friend in friends)
            {
                friendsWatchedMovies = GetAllFriendMovies(userId, friend.Id) as List<MovieJMDBApi>; // za svakog usera prijatelja uzimamo sve njegove odgledane filmove

                foreach (var movie in friendsWatchedMovies)
                {
                    if (movie.Id == movieId)
                    { // ukoliko je neki prijatelj odgledao taj film dodajemo ga u listu
                        friendsWatched.Add(friend);
                        break;
                    }
                }
            }

            return friendsWatched;
        }

        public WatchedMovie GetCommentRate(string movieId, long userId)
        {

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            var movie = _uow.WatchedMovies.FirstOrDefault(a => a.UserId == userId && a.MovieJMDBApiId == movieId);
            ValidationHelper.ValidateNotNull(movie);

            return movie;

        }

        public WatchedMovie GetFriendCommentRate(string movieId, long userId, long friendId)
        {

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            var userFriend = _uow.Users.FirstOrDefault(a => a.Id == friendId);
            ValidationHelper.ValidateNotNull(userFriend);

            //provera prijateljstva:
            var exist = _uow.Friendships.FirstOrDefault(f => (f.UserSenderId == userId && f.UserRecipientId == friendId && f.StatusCodeID == 'A') 
            || (f.UserSenderId == friendId && f.UserRecipientId == userId && f.StatusCodeID == 'A'));
            ValidationHelper.ValidateNotNull(exist);

            var movie = _uow.WatchedMovies.FirstOrDefault(a => a.UserId == friendId && a.MovieJMDBApiId == movieId);
            ValidationHelper.ValidateNotNull(movie);

            return movie;

        }

        public MovieJMDBApi GetMovie(string movieId, long userId)
        {

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId);
            ValidationHelper.ValidateNotNull(user);

            var watchedMovie = _uow.WatchedMovies.FirstOrDefault(a => a.UserId == userId && a.MovieJMDBApiId == movieId);
            ValidationHelper.ValidateNotNull(watchedMovie);

            var movie = _uow.MoviesJMDBApi.GetById(movieId);
            ValidationHelper.ValidateNotNull(movie);

            return movie;

        }

        public void DeleteMovie(string movieId, long userId)
        {

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var watchedMovieDelete = _uow.WatchedMovies.FirstOrDefault(a => a.UserId == userId && a.MovieJMDBApiId == movieId, "");
            ValidationHelper.ValidateNotNull(watchedMovieDelete);

            _uow.WatchedMovies.Delete(watchedMovieDelete); // samo brise iz odgledanih, detalji i movieAPI ostaju!
            _uow.Save();

        }

        public long CountMovies(long userId)
        {

            return _uow.WatchedMovies.Count(a => a.UserId == userId);

        }

        public MovieJMDBApi Add(MovieDetailsJMDBApi movie, long userId, string comment, int rate, string date, string poster, string movieAPIid)
        {
            // 1. dodati film u MovieJMDBApi ako ne postoji
            // 2. uneti odgledan film na kraju, pod USLOVOM da ga taj user NIJE pogledao

            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var watchedMovie = _uow.WatchedMovies.FirstOrDefault(f => f.UserId == userId && f.MovieJMDBApiId == movieAPIid, "MovieJMDBApi");
            ValidationHelper.ValidateEntityExists(watchedMovie);

            var movieAPI = _uow.MoviesJMDBApi.FirstOrDefault(a => a.Id == movieAPIid, "");
            if (movieAPI == null)
            {
                MovieJMDBApi add = new MovieJMDBApi
                {
                    Id = movieAPIid,
                    Name = movie.Name,
                    Poster = poster,
                    MovieDetailsJMDBApi = movie
                };
                _uow.MoviesJMDBApi.Add(add);
                _uow.Save();
            }

            WatchedMovie watchedMoive = new WatchedMovie
            {
                UserId = userId,
                MovieJMDBApiId = movieAPIid,
                Comment = comment,
                Rating = rate,
                WatchingDate = date,
                DateTimeAdded = DateTime.Now
                    
            };

            var watchedMovieAdded = _uow.WatchedMovies.Add(watchedMoive, "");
            _uow.Save();
            
            // we need to delete movie from saved list (if exists)
            var savedMovie = _uow.SavedMovies.FirstOrDefault(m => m.MovieJMDBApiId == movieAPIid && m.UserId == userId);

            if (savedMovie != null)
            {
                _uow.SavedMovies.Delete(savedMovie);
                _uow.Save();
            }
;
            return watchedMovieAdded.MovieJMDBApi;
        }

        public MovieJMDBApi Update(WatchedMovie movie, string movieId, long userId)
        {

            var userExists = _uow.Users.GetById(userId);
            ValidationHelper.ValidateNotNull(userExists);

            var movieExists = _uow.MoviesJMDBApi.GetById(movieId);
            ValidationHelper.ValidateNotNull(movieExists);

            // da li je taj user pogledao taj film:
            var watchedMovie = _uow.WatchedMovies.FirstOrDefault(m => m.MovieJMDBApiId == movieId && m.UserId == userId, "");
            ValidationHelper.ValidateNotNull(watchedMovie);


            if (!string.IsNullOrEmpty(movie.Comment))
                watchedMovie.Comment = movie.Comment;

            if (movie.Rating != 0)
                watchedMovie.Rating = movie.Rating;

            if (!string.IsNullOrEmpty(movie.WatchingDate))
                watchedMovie.WatchingDate = movie.WatchingDate;

            _uow.WatchedMovies.Update(watchedMovie, userId, watchedMovie.MovieJMDBApiId);
            _uow.Save();

            return movieExists;
        }
        #endregion

        #region Private Methods

        public PagedList<MovieJMDBApi> generateResult(List<MovieJMDBApi> moviesList, ResourceParameters parameters)
        {
            IQueryable<MovieJMDBApi> watchedMoviesToReturn = moviesList.AsQueryable<MovieJMDBApi>();

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var searchQuery = parameters.SearchQuery.Trim();
                watchedMoviesToReturn = watchedMoviesToReturn.Where(a => a.Name.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                // get property mapping dictionary
                var moviePropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<WatchedMovieModel, MovieJMDBApi>();

                // var m = savedMoviesToReturn.Select(movie => movie.MovieDetailsJMDBApi).ApplySort(parameters.OrderBy,
                //  moviePropertyMappingDictionary);

                watchedMoviesToReturn = watchedMoviesToReturn.ApplySort(parameters.OrderBy,
                    moviePropertyMappingDictionary);
            }

            return PagedList<MovieJMDBApi>.Create(watchedMoviesToReturn, parameters.PageNumber, parameters.PageSize);
        }

        #endregion

    }
}
