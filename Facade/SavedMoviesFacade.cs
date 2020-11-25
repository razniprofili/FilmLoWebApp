﻿using Domain;
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
            List<MovieJMDBApi> movies = SavedMoviesManager.GetAllMovies(currentUserId);
            return movies.Select(a => Mapper.Mapper.Map(a, currentUserId)).ToList();
        }

        public AddSavedMovieModel AddSavedMovie( long userId, AddSavedMovieModel addModel)
        {
            var savedMovie = SavedMoviesManager.Add(userId, Mapper.Mapper.AutoMap<AddSavedMovieModel, MovieJMDBApi>(addModel));
            return Mapper.Mapper.MapAdd(savedMovie, userId);
        }

    }
}
