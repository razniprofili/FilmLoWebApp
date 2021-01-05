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
        public List<PopularMovies> GetPopularMovies(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var popularMovies = uow.PopularMoviesRepository.Find(x => x.UserId == userId);

                return popularMovies.ToList();
            }
        }
    }
}
