using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IUnitOfWork
    {
        public IFriendshipRepository Friendships { get; set; }
        public IMovieDetailsJMDBApiRepository MoviesDetails { get; set; }
        public IMovieJMDBApiRepository MoviesJMDBApi { get; set; }
        public ISavedMovieRepository SavedMovies { get; set; }
        public IUserRepository Users { get; set; }
        public IWatchedMovieRepository WatchedMovies { get; set; }

        void Save();
       // void Dispose();
    }
}
