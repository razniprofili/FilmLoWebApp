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
        public List<WatchedMovieModel> GetAllWatchedMovies(long currentUserId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllMovies(currentUserId);
            return movies.Select(a => Mapper.Mapper.Map(a)).ToList();
        }

       public List<WatchedMovieModel> GetAllFriendMovies(long cureentUserId, long friendId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllFriendMovies(cureentUserId, friendId);
            return movies.Select(a => Mapper.Mapper.Map(a)).ToList();
        }

        public List<WatchedMovieModel> GetAllFriendsMovies(long currentUserId)
        {
            List<MovieJMDBApi> movies = WatchedMoviesManager.GetAllFriendsMovies(currentUserId);

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

        public List<UserModel> FriendsWatchThatMovie(long currentUserId, string moiveName)
        {
            List<User> friends = WatchedMoviesManager.FriendsWatchThatMovie(currentUserId, moiveName);
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
    }
}
