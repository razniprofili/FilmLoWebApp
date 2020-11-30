using FilmLoApp.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.WatchedMovies;
using Models.User;
using AutoMapper;
using Core.Services;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel] 
    [Produces("application/json")]
    [Route("api/WatchedMovies")]
    public class WatchedMoviesController : BaseController
    {
        #region Constructor
        public WatchedMoviesController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker) : base(mapper, service, checker)
        {

        }

        #endregion

        #region Routes

        [TokenAuthorize]
        [HttpGet("allMovies")]
        public List<WatchedMovieModel> GetAllMovies()
        {
            return facade.GetAllWatchedMovies(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("friendMovies/{friendId}")]
        public List<WatchedMovieModel> GetAllFriendMovies(long friendId)
        {
            return facade.GetAllFriendMovies(CurrentUser.Id, friendId);

        }

        [TokenAuthorize]
        [HttpGet("allFriendsMovies")]
        public List<WatchedMovieModel> GetAllFriendsMovies()
        {
            return facade.GetAllFriendsMovies(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpPost("friendWatched")]
        public List<UserModel> FriendsWatchThatMovie([FromBody] string moiveName)
        {
            return facade.FriendsWatchThatMovie(CurrentUser.Id, moiveName);

        }

        [TokenAuthorize]
        [HttpGet("commentRate/{movieId}")]
        public CommentRateModel GetCommentRate(string movieId)
        {
            return facade.GetCommentRate(movieId, CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("commentRate/{friendId}/{movieId}")]
        public CommentRateModel GetFriendCommentRate(string movieId, long friendId)
        {
            return facade.GetFriendCommentRate(movieId, CurrentUser.Id, friendId);
        }

        [TokenAuthorize]
        [HttpGet("getMovie/{movieId}", Name = "GetWatchedMovie")]
        public WatchedMovieModel GetMovie(string movieId)
        {
            return facade.GetWatchedMovie(movieId, CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpPut("delete/{movieId}", Name = "DeleteWatchedMovie")]
        public void DeleteMovie(string movieId)
        {
            facade.DeleteWatchedMovie(movieId, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("countMovies")]
        public long CountMovies()
        {
            return facade.CountWatchedMovies(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpPost("addWatchedMovie")]
        public WatchedMovieModel Add([FromBody] AddWatchedMovieModel movie)
        {
            return facade.AddWatchedMovie(movie, CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpPut("updateWatchedMovie/{movieId}", Name = "UpdateWatchedMovie")]
        public WatchedMovieModel Update([FromBody] UpdateWatchedMovieModel movie, string movieId)
        {
            return facade.UpdateWatchedMovie(movie, movieId, CurrentUser.Id);

        }

        #endregion

        #region Private Methods

        #endregion

    }
}
