using Common.Helpers;
using Common.ResourceParameters;
using Domain;
using Models.SavedMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    public partial class FilmLoFacade
    {
        public void DeleteSavedMovie(long currentUserId, string movieId)
        {
            SavedMoviesManager.Delete(currentUserId, movieId);
        }

        public List<SavedMovieModel> GetAllSavedMovies(long currentUserId)
        {
            List<MovieJMDBApi> movies = SavedMoviesManager.GetAllMovies(currentUserId) as List<MovieJMDBApi>;
            return movies.Select(a => Mapper.Mapper.Map(a, currentUserId)).ToList();
        }

        public PagedList<MovieJMDBApi> GetAllSavedMovies(long currentUserId, ResourceParameters usersResourceParameters)
        {
            return SavedMoviesManager.GetAllMovies(currentUserId, usersResourceParameters) as PagedList<MovieJMDBApi>;
        }

        public SavedMovieModel GetMovie(long currentUserId, string movieId)
        {
            var savedMovie = SavedMoviesManager.GetMovie(currentUserId, movieId);
            return Mapper.Mapper.Map(savedMovie, currentUserId);
        }

        public AddSavedMovieModel AddSavedMovie( long userId, AddSavedMovieModel addModel)
        {
            var savedMovie = SavedMoviesManager.Add(userId, Mapper.Mapper.Map(addModel));
            return Mapper.Mapper.MapAdd(savedMovie, userId);
        }

        public List<SavedMovieModel> SearchMovies(long currentUserId, string criteria)
        {
            List<MovieJMDBApi> movies = SavedMoviesManager.SearchMovies(currentUserId, criteria);
            return movies.Select(a => Mapper.Mapper.Map(a, currentUserId)).ToList();
        }

    }
}
