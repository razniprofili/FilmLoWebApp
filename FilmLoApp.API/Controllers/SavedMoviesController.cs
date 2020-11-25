

using FilmLoApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPut("delete/{id}")]
        public void DeleteSavedMovie(string id)
        {
            facade.DeleteSavedMovie(CurrentUser.Id, id);
        }

        [TokenAuthorize]
        [HttpGet("{id}")] // user id
        public List<SavedMovieModel> GetAllMovies(long id)
        {
            return facade.GetAllSavedMovies(id);
            
        }

        [TokenAuthorize]
        [HttpPost("add")]
        public AddSavedMovieModel AddSavedMovie([FromBody] AddSavedMovieModel savedMovieModel)
        {
            return facade.AddSavedMovie(savedMovieModel.UserId, savedMovieModel);
        }

        // Ovo moze i na front delu :)

        //[TokenAuthorize]
        //[HttpGet("search/{criteria}")]
        //public List<SavedMovieModel> SearchMoviesByName(string criteria)
        //{
        //    List<MovieJMDBApi> movies = SavedMoviesManager.SearchMovies(CurrentUser.Id, criteria);

        //    return movies.Select(a => Mapper.Map(a, CurrentUser.Id)).ToList();
        //}
    }
}
