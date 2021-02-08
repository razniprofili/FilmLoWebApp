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
        #region Managers&Services

        private IPropertyMappingService _propertyMappingService;
        internal IPropertyMappingService PropertyMappingService => _propertyMappingService ?? (_propertyMappingService = new PropertyMappingService());

        private IPropertyCheckerService _propertyCheckerService;
        internal IPropertyCheckerService PropertyCheckerService => _propertyCheckerService ?? (_propertyCheckerService = new PropertyCheckerService());

        private IUserManager UserManager;
        private IFriendshipManager FriendshipManager;
        private ISavedMoviesManager SavedMoviesManager;
        private IWatchedMoviesManager WatchedMoviesManager;
        private IPopularMoviesManager PopularMoviesManager;
        private IWatchedMoviesStatsManager WatchedMoviesStatsManager;
        private INotificationManager NotificationManager;
        private IYearStatisticManager YearStatisticManager;
        #endregion

        #region Constructors
        public FilmLoFacade(IUserManager userManager, IFriendshipManager friendshipManager)
        {
            UserManager = userManager;
            FriendshipManager = friendshipManager;
        }


        public FilmLoFacade(ISavedMoviesManager savedMoviesManager)
        {
            SavedMoviesManager = savedMoviesManager;
        }


        public FilmLoFacade(IWatchedMoviesManager watchedMoviesManager)
        {
            WatchedMoviesManager = watchedMoviesManager;
        }


        public FilmLoFacade(IPopularMoviesManager popularMoviesManager)
        {
            PopularMoviesManager = popularMoviesManager;
        }


        public FilmLoFacade(IWatchedMoviesStatsManager watchedMoviesStatsManager)
        {
            WatchedMoviesStatsManager = watchedMoviesStatsManager;
        }



        public FilmLoFacade(INotificationManager notificationManager)
        {
            NotificationManager = notificationManager;
        }


        public FilmLoFacade(IYearStatisticManager yearStatisticManager)
        {
            YearStatisticManager = yearStatisticManager;
        }
        #endregion
    }
}
