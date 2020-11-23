using Core;
using Data;
using FilmLoApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Controllers
{
    public class BaseController : Controller
    {
        internal UserJwtModel CurrentUser { get; set; }

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
