using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class UnitOfWork :  IUnitOfWork
    {

        private DbContext _context;
        public DbContext DataContext => _context ?? (_context = new FilmLoWebAppContext());

        #region Repositories

        public IFriendshipRepository Friendships { get; set; }
        public IMovieDetailsJMDBApiRepository MoviesDetails { get; set; }
        public IMovieJMDBApiRepository MoviesJMDBApi { get; set; }
        public ISavedMovieRepository SavedMovies { get; set; }
        public IUserRepository Users { get; set; }
        public IWatchedMovieRepository WatchedMovies { get; set; }
        public IPopularMoviesRepository PopularMovies { get; set; }
        public IWatchedMoviesStatsRepository WatchedMoviesStats { get; set; }
        public IYearStatisticRepository YearStatistic { get; set; }
        public INotificationRepository Notification { get; set; }

        public UnitOfWork()
        {
            Users = new UserRepository(DataContext);
            YearStatistic = new YearStatisticRepository(DataContext);
            Notification = new NotificationRepository(DataContext);
            Friendships = new FriendshipRepository(DataContext);
            WatchedMoviesStats = new WatchedMoviesStatsRepository(DataContext);
            PopularMovies = new PopularMoviesRepository(DataContext);
            SavedMovies = new SavedMovieRepository(DataContext);
            MoviesDetails = new MovieDetailsJMDBApiRepository(DataContext);
            MoviesJMDBApi = new MovieJMDBApiRepository(DataContext);
            WatchedMovies = new WatchedMovieRepository(DataContext);

        }


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
