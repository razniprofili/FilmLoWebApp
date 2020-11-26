

using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.SavedMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Controllers
{
    [ValidateModel]
    [Produces("application/json")]
    [Route("api/SavedMovies")]
    public class SavedMoviesController : BaseController
    {
        [TokenAuthorize]
        [HttpPut("delete/{id}", Name = "DeleteSavedMovie")]
        public void DeleteSavedMovie(string id)
        {
            facade.DeleteSavedMovie(CurrentUser.Id, id);
        }

        [TokenAuthorize]
        [HttpGet("{id}", Name = "GetAllSavedMovies")] // user id
        public List<SavedMovieModel> GetAllMovies(long id)
        {
            return facade.GetAllSavedMovies(id);
            
        }

        [TokenAuthorize]
        [HttpPost("add")]
        public object AddSavedMovie([FromBody] AddSavedMovieModel savedMovieModel)
        {
            // return facade.AddSavedMovie(savedMovieModel.UserId, savedMovieModel);
            var movieToReturn = facade.AddSavedMovie(savedMovieModel.UserId, savedMovieModel);

            var links = CreateLinksForMovie(movieToReturn.UserId, movieToReturn.Id);

            var linkedResourceToReturn = movieToReturn.ShapeData(null)
                as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return new { 
                          movieToReturn,
                          linkedResourceToReturn
                       };
        }

        // Ovo moze i na front delu :)

        //[TokenAuthorize]
        //[HttpGet("search/{criteria}")]
        //public List<SavedMovieModel> SearchMoviesByName(string criteria)
        //{
        //    List<MovieJMDBApi> movies = SavedMoviesManager.SearchMovies(CurrentUser.Id, criteria);

        //    return movies.Select(a => Mapper.Map(a, CurrentUser.Id)).ToList();
        //}

        #region PrivateMethods

        private object CreateLinksForMovie(long userId, string movieId)
        {
            var links = new List<LinkDto>();

            links.Add(
               new LinkDto(Url.Link("GetAllSavedMovies", new { userId }),
               "all saved movies",
               "GET"));

            links.Add(
               new LinkDto(Url.Link("DeleteSavedMovie", new { movieId }),
               "Delete saved movie.",
               "PUT"));

            return links;
        }

        #endregion
    }
}
