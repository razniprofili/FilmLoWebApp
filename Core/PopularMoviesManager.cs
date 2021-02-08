using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class PopularMoviesManager : IPopularMoviesManager
    {
        private readonly IUnitOfWork _uow;
        public PopularMoviesManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<PopularMovies> GetPopularMovies(long userId)
        {
            var user = _uow.Users.FirstOrDefault(a => a.Id == userId, "");
            ValidationHelper.ValidateNotNull(user);

            var popularMovies = _uow.PopularMovies.Find(x => x.UserId == userId, "");

            return popularMovies.ToList();

        }
    }
}
