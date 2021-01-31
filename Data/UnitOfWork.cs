using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class UnitOfWork :  IUnitOfWork
    {

        private DbContext _context;
        private DbContextOptions<FilmLoWebAppContext> _options;

        public DbContext DataContext => _context ?? (_context = new FilmLoWebAppContext()); // singlton patern

        #region Repositories

        //instaciranje repozitorijuma
        //singlton patern

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

        // private UserRepository _userRepository;
        public IUserRepository UserRepository => Users ?? (Users = new UserRepository(DataContext));

        //private FriendshipRepository _friendshipRepository;
        public IFriendshipRepository FriendshipRepository => Friendships ?? (Friendships = new FriendshipRepository(DataContext));

       // private MovieDetailsJMDBApiRepository _movieDetailsJMDBApiRepository;
        public IMovieDetailsJMDBApiRepository MovieDetailsJMDBApiRepository => MoviesDetails ?? (MoviesDetails = new MovieDetailsJMDBApiRepository(DataContext));

       // private MovieJMDBApiRepository _movieJMDBApiRepository;
        public IMovieJMDBApiRepository MovieJMDBApiRepository => MoviesJMDBApi ?? (MoviesJMDBApi = new MovieJMDBApiRepository(DataContext));

       // private SavedMovieRepository _savedMovieRepository;
        public ISavedMovieRepository SavedMovieRepository => SavedMovies ?? (SavedMovies = new SavedMovieRepository(DataContext));

       // private WatchedMovieRepository _watchedMovieRepository;
        public IWatchedMovieRepository WatchedMovieRepository => WatchedMovies ?? (WatchedMovies = new WatchedMovieRepository(DataContext));

        public IPopularMoviesRepository PopularMoviesRepository => PopularMovies ?? (PopularMovies = new PopularMoviesRepository(DataContext));

        public IWatchedMoviesStatsRepository WatchedMoviesStatsRepository => WatchedMoviesStats ?? (WatchedMoviesStats = new WatchedMoviesStatsRepository(DataContext));
        public IYearStatisticRepository YearStatisticRepository => YearStatistic ?? (YearStatistic = new YearStatisticRepository(DataContext));

        public INotificationRepository NotificationRepository => Notification ?? (Notification = new NotificationRepository(DataContext));
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
