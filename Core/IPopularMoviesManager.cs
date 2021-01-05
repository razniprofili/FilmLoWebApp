using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IPopularMoviesManager
    {
        public List<PopularMovies> GetPopularMovies(long userId);

    }
}
