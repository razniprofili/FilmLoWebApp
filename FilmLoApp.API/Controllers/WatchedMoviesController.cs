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
using Common.ResourceParameters;
using Models;
using System.Text.Json;

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
        [HttpGet("allMovies", Name = "GetAllWatchedMovies")]
        public List<WatchedMovieModel> GetAllMovies()
        {
            return facade.GetAllWatchedMovies(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("allMoviesWithParameters", Name = "GetAllWatchedMoviesWithParameters")]
        public IActionResult GetAllMovies([FromQuery]ResourceParameters parameters)
        {
            // return facade.GetAllWatchedMovies(CurrentUser.Id);

            var moviesFromrepo = facade.GetAllWatchedMovies(CurrentUser.Id, parameters);

            var paginationMetadata = new
            {
                totalCount = moviesFromrepo.TotalCount,
                pageSize = moviesFromrepo.PageSize,
                currentPage = moviesFromrepo.CurrentPage,
                totalPages = moviesFromrepo.TotalPages
                //previousPageLink,
                //nextPageLink
            };

            //dodajemo u response heder klijentu, mozee biti bilo koji format ne mora json
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            //hateoas
            var links = CreateLinksForMovie(parameters, moviesFromrepo.HasNext, moviesFromrepo.HasPrevious);

            var shapedMovies = Mapper.Mapper.MapEnumerableWatchedMovies(moviesFromrepo, CurrentUser.Id).ShapeData(parameters.Fields);

            var shapedMoviessWithLinks = shapedMovies.Select(movie =>
            {
                var movieAsDictionary = movie as IDictionary<string, object>;
                var userLinks = CreateLinksForMoviesWithFields(movieAsDictionary["Id"].ToString());
                movieAsDictionary.Add("links", userLinks);
                return movieAsDictionary;
            });

            var linkedCollectionResource = new
            {
                movies = shapedMoviessWithLinks,
                links
            };

            return Ok(linkedCollectionResource);

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

        private IEnumerable<LinkDto> CreateLinksForMovie(long userId, string movieId)
        {
            var links = new List<LinkDto>();

            links.Add(
               new LinkDto(Url.Link("GetAllWatchedMovies", new { id = "" }),
               "All watched movies for current user.",
               "GET"));

            links.Add(
               new LinkDto(Url.Link("DeleteWatchedMovie", new { movieId = movieId }),
               "Delete watched movie.",
               "PUT"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMoviesWithFields(string movieId)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(Url.Link("DeleteWatchedMovie", new { movieId = movieId }),
                "Delete saved movie.",
                "PUT"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMovie(ResourceParameters movieResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateMovieResourceUri(
                   movieResourceParameters, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateMovieResourceUri(
                      movieResourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateMovieResourceUri(
                        movieResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private string CreateMovieResourceUri(ResourceParameters movieResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAllWatchedMoviesWithParameters",
                      new
                      {
                          fields = movieResourceParameters.Fields,
                          orderBy = movieResourceParameters.OrderBy,
                          pageNumber = movieResourceParameters.PageNumber - 1,
                          pageSize = movieResourceParameters.PageSize,
                          searchQuery = movieResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAllWatchedMoviesWithParameters",
                      new
                      {
                          fields = movieResourceParameters.Fields,
                          orderBy = movieResourceParameters.OrderBy,
                          pageNumber = movieResourceParameters.PageNumber + 1,
                          pageSize = movieResourceParameters.PageSize,
                          searchQuery = movieResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current: //vazi isto sto i za def.
                default:
                    return Url.Link("GetAllWatchedMoviesWithParameters",
                    new
                    {
                        fields = movieResourceParameters.Fields,
                        orderBy = movieResourceParameters.OrderBy,
                        pageNumber = movieResourceParameters.PageNumber,
                        pageSize = movieResourceParameters.PageSize,
                        searchQuery = movieResourceParameters.SearchQuery
                    });
            }
        }

        #endregion

    }
}
