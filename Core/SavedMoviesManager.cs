using Common.Helpers;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
   public class SavedMoviesManager
    {
        public MovieJMDBApi Add(long userId, MovieJMDBApi movie)
        {
            using (var uow = new UnitOfWork())
            {
                // user mora da postoji
                var user = uow.UserRepository.FirstOrDefault(a => a.Id == userId);
                ValidationHelper.ValidateNotNull(user);

                // film mora postojati u bazi da bi se dodala asocijativna klasa, ako ne postoji dodajemo ga
                // film postoji ako vec ima taj naziv, poredimo mala slova imena

                var movieExist = uow.MovieJMDBApiRepository.FirstOrDefault(a => a.Id == movie.Id); // ovo je izgenerisan id iz jmdb APIa a ne iz nase baze!

                if (movieExist != null)
                {
                    //provera da li je vec unet film za tog usera:
                    var exist = uow.SavedMovieRepository.FirstOrDefault(f => f.UserId == userId && f.MovieJMDBApiId == movie.Id);
                    ValidationHelper.ValidateEntityExists(exist);

                    SavedMovie savedMovie = new SavedMovie
                    {
                        UserId = userId,
                        MovieJMDBApiId = movie.Id,
                        SavingDate = DateTime.UtcNow
                    };

                    uow.SavedMovieRepository.Add(savedMovie);
                    uow.Save();

                }
                else
                {
                    //moramo da ga dodamo a nakon toga i asocijativnu klasu
                    var newMovie = uow.MovieJMDBApiRepository.Add(movie);
                    uow.Save();

                    //provera da li je vec unet film za tog usera:
                    var exist = uow.SavedMovieRepository.FirstOrDefault(f => f.UserId == userId && f.MovieJMDBApiId  == movie.Id);
                    ValidationHelper.ValidateEntityExists(exist);

                    // ako nije dodajemo ga
                    SavedMovie savedMovie = new SavedMovie
                    {
                        UserId = userId,
                        MovieJMDBApiId = movie.Id,
                        SavingDate = DateTime.UtcNow
                    };

                    uow.SavedMovieRepository.Add(savedMovie);
                    uow.Save();


                }
                return movie;

            }
        }

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

                var savedMovies = uow.SavedMovieRepository.Find(m => m.UserId == userId).ToList(); //pronalazi sve sacuvane filmove za tog usera

                List<MovieJMDBApi> usersSavedMovies = new List<MovieJMDBApi>();

                foreach (var movie in savedMovies) // za sve te sacuvane filmove uzima njihove detalje
                {
                    var movieAPI = uow.MovieJMDBApiRepository.GetById(movie.MovieJMDBApiId);
                   // var movieAPI = uow.MovieJMDBApiRepository.FirstOrDefault(m => m.Id == movie.MovieJMDBApiId);
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

                var savedMovies = uow.SavedMovieRepository.Find(m => m.UserId == userId).ToList(); //pronalazi sve sacuvane filmove za tog usera

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
