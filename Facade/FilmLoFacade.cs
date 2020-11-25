using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Data;

namespace Facade
{
    public partial class FilmLoFacade
    {
        //private readonly IUserManager _userManager;
        //private readonly FriendshipManager _friendshipManager;
        //public FilmLoFacade(IUserManager userManager, FriendshipManager friendshipManager)
        //{
        //    _userManager = userManager;
        //    _friendshipManager = friendshipManager;
        //}

        private IUnitOfWork _uow;

        public IUnitOfWork IUnitOfWork => _uow ?? (_uow = new UnitOfWork());

        private UserManager _userManager;
        internal UserManager UserManager => _userManager ?? (_userManager = new UserManager());

        private SavedMoviesManager _savedMoviesManager;
        internal SavedMoviesManager SavedMoviesManager => _savedMoviesManager ?? (_savedMoviesManager = new SavedMoviesManager());

        private WatchedMoviesManager _watchedMoviesManager;
        internal WatchedMoviesManager WatchedMoviesManager => _watchedMoviesManager ?? (_watchedMoviesManager = new WatchedMoviesManager());

        private FriendshipManager _friendshipManager;
        internal FriendshipManager FriendshipManager => _friendshipManager ?? (_friendshipManager = new FriendshipManager());

    }
}
