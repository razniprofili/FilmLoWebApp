

using AutoMapper;
using Common.ResourceParameters;
using Core;
using Core.Services;
using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.SavedMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel]
    [Produces("application/json")]
    [Route("api/SavedMovies")]
    public class SavedMoviesController : BaseController
    {
        #region Constructor
        public SavedMoviesController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker,
            ISavedMoviesManager savedMoviesManager) 
            : base(mapper, service, checker, savedMoviesManager)
        {

        }

        #endregion

        #region Routes

        [TokenAuthorize]
        [HttpPut("delete/{id}", Name = "DeleteSavedMovie")]
        public void DeleteSavedMovie(string id)
        {
            facadeSavedMovies.DeleteSavedMovie(CurrentUser.Id, id);
        }

        [TokenAuthorize]
        [HttpGet( Name = "GetAllSavedMovies")]
        public List<SavedMovieModel> GetAllSavedMovies()
        {
            return facadeSavedMovies.GetAllSavedMovies(CurrentUser.Id);

        }

        [TokenAuthorize]
        [HttpGet("getAllMoviesWithParameters", Name = "GetAllSavedMovieswithParameters")]
        public IActionResult GetAllSavedMovies([FromQuery] ResourceParameters parameters)
        {
            if (parameters.Fields != null && !parameters.Fields.ToLower().Contains("id"))
            {
                return BadRequest("Result must include Id.");

            }
            else
            {
                var moviesFromrepo = facadeSavedMovies.GetAllSavedMovies(CurrentUser.Id, parameters);

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

                var shapedMovies = Mapper.Mapper.Map(moviesFromrepo, CurrentUser.Id).ShapeData(parameters.Fields);

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
        [HttpGet("{movieId}", Name = "GetMovie")]
        public ActionResult<SavedMovieModel> GetMovie(string movieId)
        {
            var movieIdToReturn = facadeSavedMovies.GetMovie(CurrentUser.Id, movieId);
            var links = CreateLinksForMovie(CurrentUser.Id, movieId);

            return generateResult(movieIdToReturn, links);
        }

        [TokenAuthorize]
        [HttpPost("add")]
        public object AddSavedMovie([FromBody] AddSavedMovieModel savedMovieModel)
        {
            if (savedMovieModel.UserId != CurrentUser.Id)
                return BadRequest($"Movie can be added only for current user: {CurrentUser.Name} {CurrentUser.Surname}({CurrentUser.Mail})");

            var movieToReturn = facadeSavedMovies.AddSavedMovie(savedMovieModel.UserId, savedMovieModel);

            var links = CreateLinksForMovie(movieToReturn.UserId, movieToReturn.Id);

            var addedMovie = movieToReturn.ShapeData(null)
                as IDictionary<string, object>;
            addedMovie.Add("links", links);

            return new
            {
                addedMovie
            };
        }


        [TokenAuthorize]
        [HttpGet("search/{criteria}")]
        public List<SavedMovieModel> SearchMoviesByName(string criteria)
        {
            return facadeSavedMovies.SearchMovies(CurrentUser.Id, criteria);

        }

        #endregion

        #region Private Methods

        private ActionResult<SavedMovieModel> generateResult(SavedMovieModel movieToReturn, IEnumerable<LinkDto> links)
        {
            var linkedResourceToReturn = movieToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetMovie",
                new { movieId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }

        private IEnumerable<LinkDto> CreateLinksForMovie(long userId, string movieId)
        {
            var links = new List<LinkDto>();

            links.Add(
               new LinkDto(Url.Link("GetAllSavedMovies", new { id = "" }),
               "All saved movies for current user.",
               "GET"));

            links.Add(
               new LinkDto(Url.Link("DeleteSavedMovie", new { id = movieId }),
               "Delete saved movie.",
               "PUT"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMoviesWithFields(string movieId)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(Url.Link("DeleteSavedMovie", new { id = movieId }),
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
                    return Url.Link("GetAllSavedMovieswithParameters",
                      new
                      {
                          fields = movieResourceParameters.Fields,
                          orderBy = movieResourceParameters.OrderBy,
                          pageNumber = movieResourceParameters.PageNumber - 1,
                          pageSize = movieResourceParameters.PageSize,
                          searchQuery = movieResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAllSavedMovieswithParameters",
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
                    return Url.Link("GetAllSavedMovieswithParameters",
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
