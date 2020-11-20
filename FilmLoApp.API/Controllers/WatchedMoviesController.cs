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
    [ValidateModel] 
    [Produces("application/json")]
    [Route("api/WatchedMovies")]
    public class WatchedMoviesController : BaseController
    {
        [TokenAuthorize]
        [HttpGet("{id}")] // user id
        public List<WatchedMovieModel> GetAllMovies(long id)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllMovies(id);
            return movies.Select(a => Mapper.Map(a)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("{id}/{friendId}")]
        public List<WatchedMovieModel> GetAllFriendMovies(long id, long friendId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllFriendMovies(id, friendId);
            return movies.Select(a => Mapper.Map(a)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("allMovies/{id}")] // user Id
        public List<WatchedMovieModel> GetAllFriendsMovies(long id)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllFriendsMovies(id);

            List<WatchedMovieModel> moviesToReturn = new List<WatchedMovieModel>();

            foreach (var movie in movies)
            {
                foreach (var watchedMovie in movie.WatchedUsers)
                {
                    var addMovie = Mapper.MapFriend(movie, watchedMovie);

                    if (!moviesToReturn.Exists(x => x.Name == addMovie.Name && x.UserId == addMovie.UserId))
                    {
                        moviesToReturn.Add(addMovie);
                    }
                }
            }

            return moviesToReturn;
        }

        [TokenAuthorize]
        [HttpPost("friendWatched/{id}")] // user Id
        public List<UserModel> FriendsWatchThatMovie([FromBody] string moiveName, long id)
        {
            List<User> friends = WatchedMoviesManager.FriendsWatchThatMovie(id, moiveName);
            return friends.Select(a => Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        [TokenAuthorize]
        [HttpGet("commentRate/{id}/{movieId}")]
        public CommentRateModel GetCommentRate(long id, string movieId)
        {
            var movie = WatchedMoviesManager.GetCommentRate(movieId, id);
            return Mapper.Map(movie);
        }

        [TokenAuthorize]
        [HttpGet("{id}/{friendId}/{movieId}")]
        public CommentRateModel GetFriendCommentRate(string movieId, long id, long friendId)
        {
            var movie = WatchedMoviesManager.GetFriendCommentRate(movieId, id, friendId);
            return Mapper.Map(movie);
        }

        [TokenAuthorize]
        [HttpGet("getMovie/{movieId}")]
        public WatchedMovieModel GetMovie(string movieId)
        {
            var movie = WatchedMoviesManager.GetMovie(movieId, CurrentUser.Id);
            return Mapper.Map(movie);
        }

        [TokenAuthorize]
        [HttpPut("delete/{movieId}")]
        public void DeleteMovie(string movieId)
        {
            WatchedMoviesManager.DeleteMovie(movieId, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet]
        public long CountMovies()
        {
            return WatchedMoviesManager.CountMovies(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpPost("addWatchedMovie")]
        public WatchedMovieModel Add([FromBody] AddWatchedMovieModel movie)
        {
            var addedMovie = WatchedMoviesManager.Add(Mapper.MapWatchedMovie(movie), CurrentUser.Id, movie.Comment, movie.Rate, movie.DateTimeWatched, movie.Poster, movie.Id);
            return Mapper.Map(addedMovie);
        }

        [TokenAuthorize]
        [HttpPut("updateWatchedMovie/{movieId}")] 
        public WatchedMovieModel Update([FromBody] UpdateWatchedMovieModel movie, string movieId)
        {
            var updatedMovie = WatchedMoviesManager.Update(Mapper.MapUpdate(movie), movieId, CurrentUser.Id);
            return Mapper.Map(updatedMovie);
        }
    }
}
