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


       // private IUnitOfWork _uow;
       // public IUnitOfWork IUnitOfWork => _uow ?? (_uow = new UnitOfWork());

        private IUserManager _userManager;
        internal IUserManager UserManager => _userManager ?? (_userManager = new UserManager(PropertyMappingService, PropertyCheckerService));

        private ISavedMoviesManager _savedMoviesManager;
        internal ISavedMoviesManager SavedMoviesManager => _savedMoviesManager ?? (_savedMoviesManager = new SavedMoviesManager(PropertyMappingService, PropertyCheckerService));

        private IWatchedMoviesManager _watchedMoviesManager;
        internal IWatchedMoviesManager WatchedMoviesManager => _watchedMoviesManager ?? (_watchedMoviesManager = new WatchedMoviesManager(PropertyMappingService, PropertyCheckerService));

        private IFriendshipManager _friendshipManager;
        internal IFriendshipManager FriendshipManager => _friendshipManager ?? (_friendshipManager = new FriendshipManager(PropertyMappingService, PropertyCheckerService));

        private IPopularMoviesManager _popularMoviesManager;
        internal IPopularMoviesManager PopularMoviesManager => _popularMoviesManager ?? (_popularMoviesManager = new PopularMoviesManager());

        private IWatchedMoviesStatsManager _watchedMoviesStatsManager;
        internal IWatchedMoviesStatsManager WatchedMoviesStatsManager => _watchedMoviesStatsManager ?? (_watchedMoviesStatsManager = new WatchedMoviesStatsManager());
    }
}
