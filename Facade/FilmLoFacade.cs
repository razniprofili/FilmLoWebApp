using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Services;
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

        private IPropertyMappingService _propertyMappingService;
        internal IPropertyMappingService PropertyMappingService => _propertyMappingService ?? (_propertyMappingService = new PropertyMappingService());

        private IPropertyCheckerService _propertyCheckerService;
        internal IPropertyCheckerService PropertyCheckerService => _propertyCheckerService ?? (_propertyCheckerService = new PropertyCheckerService());


        private IUnitOfWork _uow;

        public IUnitOfWork IUnitOfWork => _uow ?? (_uow = new UnitOfWork());

        private UserManager _userManager;
        internal UserManager UserManager => _userManager ?? (_userManager = new UserManager(PropertyMappingService, PropertyCheckerService));

        private SavedMoviesManager _savedMoviesManager;
        internal SavedMoviesManager SavedMoviesManager => _savedMoviesManager ?? (_savedMoviesManager = new SavedMoviesManager());

        private WatchedMoviesManager _watchedMoviesManager;
        internal WatchedMoviesManager WatchedMoviesManager => _watchedMoviesManager ?? (_watchedMoviesManager = new WatchedMoviesManager());

        private FriendshipManager _friendshipManager;
        internal FriendshipManager FriendshipManager => _friendshipManager ?? (_friendshipManager = new FriendshipManager(PropertyMappingService, PropertyCheckerService));

    }
}
