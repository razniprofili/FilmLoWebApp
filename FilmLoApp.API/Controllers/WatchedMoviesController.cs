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
using Core;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel] 
    [Produces("application/json")]
    [Route("api/WatchedMovies")]
    public class WatchedMoviesController : BaseController
    {
        #region Constructor
        public WatchedMoviesController(IMapper mapper, IPropertyMappingService service, 
            IPropertyCheckerService checker, IYearStatisticManager yearStatisticManager,
            IWatchedMoviesStatsManager watchedMoviesStatsManager, IPopularMoviesManager popularMoviesManager,
            IWatchedMoviesManager watchedMoviesManager) 
            : base(mapper, service, checker, yearStatisticManager, watchedMoviesStatsManager, popularMoviesManager,
                  watchedMoviesManager)
        {

        }

        #endregion

        #region Routes

        [TokenAuthorize]
        [HttpGet("allMovies", Name = "GetAllWatchedMovies")]
        public List<WatchedMovieModel> GetAllMovies()
        {
            return facadeWatchedMovies.GetAllWatchedMovies(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("allMoviesWithParameters", Name = "GetAllWatchedMoviesWithParameters")]
        public IActionResult GetAllMovies([FromQuery]ResourceParameters parameters)
        {
            if (parameters.Fields != null && !parameters.Fields.ToLower().Contains("id"))
            {
                return BadRequest("Result must include Id.");

            }
            else
            {
                var moviesFromrepo = facadeWatchedMovies.GetAllWatchedMovies(CurrentUser.Id, parameters);

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
                var links = CreateLinksForMovie(parameters, moviesFromrepo.HasNext, moviesFromrepo.HasPrevious, "GetAllWatchedMoviesWithParameters");

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
                
        }

        [TokenAuthorize]
        [HttpGet("friendMovies/{friendId}")]
        public List<WatchedMovieModel> GetAllFriendMovies(long friendId)
        {
            return facadeWatchedMovies.GetAllFriendMovies(CurrentUser.Id, friendId);

        }

        [TokenAuthorize]
        [HttpGet("friendMoviesWithParameters/{friendId}", Name = "GetAllWatchedMoviesForFriendWithParameters")]
        public IActionResult GetAllFriendMovies(long friendId, [FromQuery] ResourceParameters parameters)
        {
            if (parameters.Fields != null && !parameters.Fields.ToLower().Contains("id"))
            {
                return BadRequest("Result must include Id.");

            }
            else
            {
                var moviesFromrepo = facadeWatchedMovies.GetAllFriendMovies(CurrentUser.Id, friendId, parameters);

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
                var links = CreateLinksForMovie(parameters, moviesFromrepo.HasNext, moviesFromrepo.HasPrevious, "GetAllWatchedMoviesForFriendWithParameters");

                var shapedMovies = Mapper.Mapper.MapEnumerableWatchedMovies(moviesFromrepo, CurrentUser.Id).ShapeData(parameters.Fields);

                var shapedMoviessWithLinks = shapedMovies.Select(movie =>
                {
                    var movieAsDictionary = movie as IDictionary<string, object>;
                    // var userLinks = CreateLinksForMoviesWithFields(movieAsDictionary["Id"].ToString());
                    // movieAsDictionary.Add("links", userLinks);
                    return movieAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    movies = shapedMoviessWithLinks,
                    links
                };

                return Ok(linkedCollectionResource);
            }
               
        }


        [TokenAuthorize]
        [HttpGet("allFriendsMovies")]
        public List<WatchedMovieModel> GetAllFriendsMovies()
        {
            return facadeWatchedMovies.GetAllFriendsMovies(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("allFriendsMoviesWithParameters", Name = "GetAllFriendsMovies")]
        public IActionResult GetAllFriendsMovies([FromQuery] ResourceParameters parameters)
        {
            if (parameters.Fields != null && !parameters.Fields.ToLower().Contains("id"))
            {
                return BadRequest("Result must include Id.");

            }
            else
            {
                var moviesFromrepo = facadeWatchedMovies.GetAllFriendsMovies(CurrentUser.Id, parameters);

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
                var links = CreateLinksForMovie(parameters, moviesFromrepo.HasNext, moviesFromrepo.HasPrevious, "GetAllFriendsMovies");

                var shapedMovies = Mapper.Mapper.MapEnumerableWatchedMovies(moviesFromrepo, CurrentUser.Id).ShapeData(parameters.Fields);

                var shapedMoviessWithLinks = shapedMovies.Select(movie =>
                {
                    var movieAsDictionary = movie as IDictionary<string, object>;
                    // var userLinks = CreateLinksForMoviesWithFields(movieAsDictionary["Id"].ToString());
                    // movieAsDictionary.Add("links", userLinks);
                    return movieAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    movies = shapedMoviessWithLinks,
                    links
                };

                return Ok(linkedCollectionResource);
            }
                
        }


        [TokenAuthorize]
        [HttpGet("friendWatched/{movieId}")]
        public List<UserModel> FriendsWatchThatMovie(string movieId)
        {
            return facadeWatchedMovies.FriendsWatchThatMovie(CurrentUser.Id, movieId);

        }

        [TokenAuthorize]
        [HttpGet("commentRate/{movieId}")]
        public CommentRateModel GetCommentRate(string movieId)
        {
            return facadeWatchedMovies.GetCommentRate(movieId, CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("commentRate/{friendId}/{movieId}")]
        public CommentRateModel GetFriendCommentRate(string movieId, long friendId)
        {
            return facadeWatchedMovies.GetFriendCommentRate(movieId, CurrentUser.Id, friendId);
        }

        [TokenAuthorize]
        [HttpGet("getMovie/{movieId}", Name = "GetWatchedMovie")]
        public ActionResult<WatchedMovieModel> GetMovie(string movieId)
        {

            var movieToReturn = facadeWatchedMovies.GetWatchedMovie(movieId, CurrentUser.Id);
            var links = CreateLinksForMovie(CurrentUser.Id, movieToReturn.Id);

            return generateResult(movieToReturn, links);
        }

        [TokenAuthorize]
        [HttpPut("delete/{movieId}", Name = "DeleteWatchedMovie")]
        public void DeleteMovie(string movieId)
        {
            facadeWatchedMovies.DeleteWatchedMovie(movieId, CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("countMovies", Name = "CountWatchedMovies")]
        public long CountMovies()
        {
            return facadeWatchedMovies.CountWatchedMovies(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpPost("addWatchedMovie")]
        public ActionResult<WatchedMovieModel> Add([FromBody] AddWatchedMovieModel movie)
        {

            var movieToReturn = facadeWatchedMovies.AddWatchedMovie(movie, CurrentUser.Id);
            var links = CreateLinksForMovie(CurrentUser.Id, movieToReturn.Id);

            return generateResult(movieToReturn, links);
        }

        [TokenAuthorize]
        [HttpPut("updateWatchedMovie/{movieId}", Name = "UpdateWatchedMovie")]
        public ActionResult<WatchedMovieModel> Update([FromBody] UpdateWatchedMovieModel movie, string movieId)
        {
            var movieToReturn = facadeWatchedMovies.UpdateWatchedMovie(movie, movieId, CurrentUser.Id);
            var links = CreateLinksForMovie(CurrentUser.Id, movieToReturn.Id);

            return generateResult(movieToReturn, links);

        }

        [TokenAuthorize]
        [HttpGet("popularMovies")]
        public List<PopularMoviesModel> GetPopularMovies()
        {
            return facadePopularMovies.GetPopularMovies(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("watchedMoviesStats")]
        public WatchedMoviesStatsModel GetWatchedMoviesStats()
        {
            return facadeWatchedMoviesStats.GetWatchedMoviesStats(CurrentUser.Id);
        }

        [TokenAuthorize]
        [HttpGet("yearStatistic")]
        public List<YearStatisticModel> GetYearStatistic()
        {
            return facadeYearStats.GetYearStatistic(CurrentUser.Id);
        }

        #endregion

        #region Private Methods

        private ActionResult<WatchedMovieModel> generateResult(WatchedMovieModel movieToReturn, IEnumerable<LinkDto> links)
        {
            var linkedResourceToReturn = movieToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetWatchedMovie",
                new { movieId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }

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

            links.Add(
               new LinkDto(Url.Link("UpdateWatchedMovie", new { movieId = movieId }),
               "Update watched movie. You must enter fields for update in the body! Empty body -> no update",
               "PUT"));

            links.Add(
               new LinkDto(Url.Link("CountWatchedMovies", new { id = "" }),
               "Number of watched movies for current user.",
               "GET"));


            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMoviesWithFields(string movieId)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(Url.Link("DeleteWatchedMovie", new { movieId = movieId }),
                "Delete watched movie.",
                "PUT"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMovie(ResourceParameters movieResourceParameters, bool hasNext, bool hasPrevious, string routeName)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateMovieResourceUri(
                   movieResourceParameters, ResourceUriType.Current, routeName)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateMovieResourceUri(
                      movieResourceParameters, ResourceUriType.NextPage, routeName),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateMovieResourceUri(
                        movieResourceParameters, ResourceUriType.PreviousPage, routeName),
                    "previousPage", "GET"));
            }

            return links;
        }

        private string CreateMovieResourceUri(ResourceParameters movieResourceParameters, ResourceUriType type, string routeName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                      new
                      {
                          fields = movieResourceParameters.Fields,
                          orderBy = movieResourceParameters.OrderBy,
                          pageNumber = movieResourceParameters.PageNumber - 1,
                          pageSize = movieResourceParameters.PageSize,
                          searchQuery = movieResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
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
                    return Url.Link(routeName,
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
