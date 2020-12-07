using Common.ResourceParameters;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface ISavedMoviesManager
    {
        public List<MovieJMDBApi> SearchMovies(long userId, string critearia);
        public object GetAllMovies(long userId, ResourceParameters parameters = null);
        public void Delete(long userId, string movieId);
        public MovieJMDBApi Add(long userId, MovieJMDBApi movie);

    }
}
