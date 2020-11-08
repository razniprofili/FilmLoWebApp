using FilmLoApp.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilmLoApp.API.Models.WatchedMovies;
using Domain;
using FilmLoApp.API.Models.User;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel] // to je ono sto smo pisali u Helpers folderu
    [Produces("application/json")]
    [Route("api/WatchedMovies")]
    public class WatchedMoviesController : BaseController
    {
        [TokenAuthorize]
        [HttpGet("{id}")] // user id
        public List<WatchedMovieModel> GetAllMovies(long id) 
        {
            List<MovieDetailsJMDBApi> movies = WatchedMoviesManager.GetAllMovies(id);
            return movies.Select(a => Mapper.Map(a, id)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("{id}/{friendId}")] 
        public List<WatchedMovieModel> GetAllFriendMovies(long id, long friendId) 
        {
            List<MovieDetailsJMDBApi> movies = WatchedMoviesManager.GetAllFriendMovies(id, friendId);
            return movies.Select(a => Mapper.Map(a, friendId)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("allMovies/{id}")] // user Id
        public List<WatchedMovieModel> GetAllFriendsMovies(long id)
        {
            List<MovieDetailsJMDBApi> movies = WatchedMoviesManager.GetAllFriendsMovies(id);
            return movies.Select(a => Mapper.Map(a)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("friendWatched/{id}")] // user Id
        public List<UserModel> FriendsWatchThatMovie([FromBody] string moiveName, long id)
        {
            List<User> friends = WatchedMoviesManager.FriendsWatchThatMovie(id, moiveName);
            return friends.Select(a => Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("{id}/{movieId}")]
        public CommentRateModel GetCommentRate(long id, long movieId)
        {
            var movie = WatchedMoviesManager.GetCommentRate(movieId, id);
            return Mapper.Map(movie);
        }

        [TokenAuthorize]
        [HttpGet("{id}/{friendId}/{movieId}")]
        public CommentRateModel GetFriendCommentRate(long movieId, long id, long friendId)
        {
            var movie = WatchedMoviesManager.GetFriendCommentRate(movieId, id, friendId);
            return Mapper.Map(movie);
        }

        [TokenAuthorize]
        [HttpGet("getMovie/{id}/{movieId}")]
        public WatchedMovieModel GetMovie(long movieId, long id)
        {
            var movie = WatchedMoviesManager.GetMovie(movieId, id);
            return Mapper.AutoMap<MovieDetailsJMDBApi, WatchedMovieModel>(movie);
        }

        [TokenAuthorize]
        [HttpPut("{id}/{movieId}")]
        public void DeleteMovie(long movieId, long id)
        {
            WatchedMoviesManager.DeleteMovie(movieId, id);
        }

        [TokenAuthorize]
        [HttpGet]
        public long CountMovies()
        {
            return WatchedMoviesManager.CountMovies(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpPost("{comment}/{rate}/{date}")]
        public WatchedMovieModel Add([FromBody]WatchedMovieModel movie, string comment, int rate, string date)
        {
            var addedMovie = WatchedMoviesManager.Add(Mapper.AutoMap<WatchedMovieModel, MovieDetailsJMDBApi>(movie), CurrentUser.Id, comment, rate, date);
            return Mapper.AutoMap<MovieDetailsJMDBApi, WatchedMovieModel>(addedMovie);
        }

        // update


    }
}
