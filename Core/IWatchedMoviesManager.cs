using Common.ResourceParameters;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IWatchedMoviesManager
    {
        public MovieJMDBApi Update(WatchedMovie movie, string movieId, long userId);
        public MovieJMDBApi Add(MovieDetailsJMDBApi movie, long userId, string comment, int rate, string date, string poster, string movieAPIid);
        public long CountMovies(long userId);
        public void DeleteMovie(string movieId, long userId);
        public MovieJMDBApi GetMovie(string movieId, long userId);
        public WatchedMovie GetFriendCommentRate(string movieId, long userId, long friendId);
        public WatchedMovie GetCommentRate(string movieId, long userId);
        public List<User> FriendsWatchThatMovie(long userId, string movieId);
        public object GetAllFriendsMovies(long userId, ResourceParameters parameters = null);
        public object GetAllFriendMovies(long userId, long friendId, ResourceParameters parameters = null);
        public object GetAllMovies(long userId, ResourceParameters parameters = null);
    }
}
