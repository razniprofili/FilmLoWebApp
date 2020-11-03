using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
   public class SavedMoviesManager
    {
        //public SavedMovie AddMovie(long userId, MovieSavingModel savingMovie)
        //{
              
        //}

        public void Delete(long userId, string movieId)
        {
            using (var uow = new UnitOfWork())
            {

                // proveravamo da li postoji sacuvan film, po idju usera i idju filma koji se brise
                var movieToDelete = uow.SavedMovieRepository.FirstOrDefault(p => p.UserId == userId && p.MovieJMDBApiId == movieId);
                ValidationHelper.ValidateNotNull(movieToDelete); 

                uow.SavedMovieRepository.Delete(movieToDelete);
                uow.Save();
            }
        }

        public List<MovieJMDBApi> GetAllMovies(long userId)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user= uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var savedMovies = uow.SavedMovieRepository.Find(m => m.UserId == userId); //pronalazi sve sacuvane filmove za tog usera

                List<MovieJMDBApi> usersSavedMovies = new List<MovieJMDBApi>();

                foreach (var movie in savedMovies) // za sve te sacuvane filmove uzima njihove detalje
                {
                    var movieAPI = uow.MovieJMDBApiRepository.GetById(movie.MovieJMDBApiId);
                    usersSavedMovies.Add(movieAPI);
                }

                return usersSavedMovies;
            }
        }

        public List<MovieJMDBApi> SearchMovies(long userId, string critearia)
        {
            using (var uow = new UnitOfWork())
            {
                //provera da li postoji user za svaki slucaj:
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                var savedMovies = uow.SavedMovieRepository.Find(m => m.UserId == userId); //pronalazi sve sacuvane filmove za tog usera

                List<MovieJMDBApi> usersSavedMovies = new List<MovieJMDBApi>();

                foreach (var movie in savedMovies) // za sve te sacuvane filmove uzima njihove detalje
                {
                    var movieAPI = uow.MovieJMDBApiRepository.FirstOrDefault( m => m.Id == movie.MovieJMDBApiId && m.Name==critearia);
                    usersSavedMovies.Add(movieAPI);
                }

                return usersSavedMovies;
            }
        }

    }
}
