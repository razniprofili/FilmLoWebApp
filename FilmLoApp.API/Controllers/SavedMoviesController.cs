using Core;
using Domain;
using FilmLoApp.API.Helpers;
using FilmLoApp.API.Models.SavedMovies;
using Microsoft.AspNetCore.Mvc;
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
            SavedMoviesManager.Delete(CurrentUser.Id, id);
        }

        [TokenAuthorize]
        [HttpGet("{id}")] // user id
        public List<SavedMovieModel> GetAllMovies(long id)
        {
            List<MovieJMDBApi> movies = SavedMoviesManager.GetAllMovies(id);
            return movies.Select(a => Mapper.Map(a, id)).ToList();
        }

        [TokenAuthorize]
        [HttpPost("add")]
        public AddSavedMovieModel AddSavedMovie([FromBody]AddSavedMovieModel savedMovieModel)
        {
            var savedMovie = SavedMoviesManager.Add(savedMovieModel.UserId, Mapper.AutoMap<AddSavedMovieModel, MovieJMDBApi>(savedMovieModel));
            // return Mapper.AutoMap<MovieJMDBApi, AddSavedMovieModel>(savedMovie);
            return Mapper.MapAdd(savedMovie, CurrentUser.Id);
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
