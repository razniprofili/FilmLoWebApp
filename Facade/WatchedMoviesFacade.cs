using Common.Helpers;
using Common.ResourceParameters;
using Domain;
using Models.User;
using Models.WatchedMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    public partial class FilmLoFacade
    {
        #region Get All

        public List<WatchedMovieModel> GetAllWatchedMovies(long currentUserId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllMovies(currentUserId) as List<MovieJMDBApi>;
            return movies.Select(a => Mapper.Mapper.Map(a)).ToList();
        }

        public PagedList<MovieJMDBApi> GetAllWatchedMovies(long currentUserId, ResourceParameters moviesResourceParameters)
        {
            return WatchedMoviesManager.GetAllMovies(currentUserId, moviesResourceParameters) as PagedList<MovieJMDBApi>;
        }

        public List<WatchedMovieModel> GetAllFriendMovies(long cureentUserId, long friendId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllFriendMovies(cureentUserId, friendId) as List<MovieJMDBApi>;
            return movies.Select(a => Mapper.Mapper.Map(a)).ToList();
        }

        public PagedList<MovieJMDBApi> GetAllFriendMovies(long cureentUserId, long friendId, ResourceParameters moviesResourceParameters)
        {
            return WatchedMoviesManager.GetAllFriendMovies(cureentUserId, friendId, moviesResourceParameters) as PagedList<MovieJMDBApi>;

        }

        public List<WatchedMovieModel> GetAllFriendsMovies(long currentUserId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllFriendsMovies(currentUserId) as List<MovieJMDBApi>;

            List<WatchedMovieModel> moviesToReturn = new List<WatchedMovieModel>();

            foreach (var movie in movies)
            {
                foreach (var watchedMovie in movie.WatchedUsers)
                {
                    var addMovie = Mapper.Mapper.MapFriend(movie, watchedMovie);

                    if (!moviesToReturn.Exists(x => x.Name == addMovie.Name && x.UserId == addMovie.UserId))
                    {
                        moviesToReturn.Add(addMovie);
                    }
                }
            }

            return moviesToReturn;
        }

        public PagedList<MovieJMDBApi> GetAllFriendsMovies(long currentUserId, ResourceParameters parameters)
        {
            return WatchedMoviesManager.GetAllFriendsMovies(currentUserId, parameters) as PagedList<MovieJMDBApi>;
        }
        #endregion

        public List<UserModel> FriendsWatchThatMovie(long currentUserId, string movieId)
        {
            List<User> friends = WatchedMoviesManager.FriendsWatchThatMovie(currentUserId, movieId);
            return friends.Select(a => Mapper.Mapper.AutoMap<User, UserModel>(a)).ToList();
        }

        public CommentRateModel GetCommentRate(string movieId, long currentUserId)
        {
            var movie = WatchedMoviesManager.GetCommentRate(movieId, currentUserId);
            return Mapper.Mapper.Map(movie);
        }

        public CommentRateModel GetFriendCommentRate(string movieId, long currentUserId, long friendId)
        {
            var movie = WatchedMoviesManager.GetFriendCommentRate(movieId, currentUserId, friendId);
            return Mapper.Mapper.Map(movie);
        }

        public WatchedMovieModel GetWatchedMovie(string movieId, long currentUserId)
        {
            var movie = WatchedMoviesManager.GetMovie(movieId, currentUserId);
            return Mapper.Mapper.Map(movie);
        }

        public void DeleteWatchedMovie(string movieId, long currentUserId)
        {
            WatchedMoviesManager.DeleteMovie(movieId, currentUserId);
        }

        public long CountWatchedMovies( long currentUserId)
        {
            return WatchedMoviesManager.CountMovies(currentUserId);
        }

        public WatchedMovieModel AddWatchedMovie(AddWatchedMovieModel movie, long currentUserId)
        {
            var addedMovie = WatchedMoviesManager.Add(Mapper.Mapper.MapWatchedMovie(movie), currentUserId, movie.Comment, movie.Rate, movie.DateTimeWatched, movie.Poster, movie.Id);
            return Mapper.Mapper.Map(addedMovie);
        }

        public WatchedMovieModel UpdateWatchedMovie(UpdateWatchedMovieModel movieModel, string movieId, long currentUserid)
        {
            var updatedMovie = WatchedMoviesManager.Update(Mapper.Mapper.MapUpdate(movieModel), movieId, currentUserid);
            return Mapper.Mapper.Map(updatedMovie);
        }

        public List<PopularMoviesModel> GetPopularMovies(long currentUserId)
        {
            var popularMovies = PopularMoviesManager.GetPopularMovies(currentUserId);

            return popularMovies.Select(m => Mapper.Mapper.AutoMap<PopularMovies, PopularMoviesModel>(m)).ToList();
        }

        public WatchedMoviesStatsModel GetWatchedMoviesStats (long currentUserId)
        {
            return Mapper.Mapper.AutoMap<WatchedMoviesStats, WatchedMoviesStatsModel>(WatchedMoviesStatsManager.GetWatchedMoviesStats(currentUserId));
        }

        public List<YearStatisticModel> GetYearStatistic(long currentUserId)
        {
            var yearStatistic = YearStatisticManager.GetYearStatistic(currentUserId);

            return yearStatistic.Select(m => Mapper.Mapper.AutoMap<YearStatistic, YearStatisticModel>(m)).ToList();
        }

    }
}
