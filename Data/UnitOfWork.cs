using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class UnitOfWork : IDisposable
    {

        private DbContext _context;
        private DbContextOptions<FilmLoWebAppContext> _options;

        public DbContext DataContext => _context ?? (_context = new FilmLoWebAppContext()); // singlton patern

        #region Repositories

        //instaciranje repozitorijuma
        //singlton patern

        private UserRepository _userRepository;
        public UserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(DataContext));

        private FriendshipRepository _friendshipRepository;
        public FriendshipRepository FriendshipRepository => _friendshipRepository ?? (_friendshipRepository = new FriendshipRepository(DataContext));

        private MovieDetailsJMDBApiRepository _movieDetailsJMDBApiRepository;
        public MovieDetailsJMDBApiRepository MovieDetailsJMDBApiRepository => _movieDetailsJMDBApiRepository ?? (_movieDetailsJMDBApiRepository = new MovieDetailsJMDBApiRepository(DataContext));

        private MovieJMDBApiRepository _movieJMDBApiRepository;
        public MovieJMDBApiRepository MovieJMDBApiRepository => _movieJMDBApiRepository ?? (_movieJMDBApiRepository = new MovieJMDBApiRepository(DataContext));

        private SavedMovieRepository _savedMovieRepository;
        public SavedMovieRepository SavedMovieRepository => _savedMovieRepository ?? (_savedMovieRepository = new SavedMovieRepository(DataContext));

        private WatchedMovieRepository _watchedMovieRepository;
        public WatchedMovieRepository WatchedMovieRepository => _watchedMovieRepository ?? (_watchedMovieRepository = new WatchedMovieRepository(DataContext));

        #endregion

        public virtual void Save()
        {
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                    if (_context != null)
                        _context.Dispose();
                }
            }
            _disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
