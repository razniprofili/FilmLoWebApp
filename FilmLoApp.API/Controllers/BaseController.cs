
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facade;
using Models;
using AutoMapper;
using Core.Services;
using Core;

namespace FilmLoApp.API.Controllers
{
    public class BaseController : Controller
    {     
        internal UserJwtModel CurrentUser { get; set; }

        #region Managers&Services
        internal readonly IMapper _mapper;
        internal readonly IPropertyMappingService _service;
        internal readonly IPropertyCheckerService _servicePropertyChecker;
        internal readonly IYearStatisticManager _yearStatisticManager;
        internal readonly INotificationManager _notificationManager;
        internal readonly IWatchedMoviesStatsManager _watchedMoviesStatsManager;
        internal readonly IPopularMoviesManager _popularMoviesManager;
        internal readonly ISavedMoviesManager _savedMoviesManager;
        internal readonly IUserManager _userManager;
        internal readonly IFriendshipManager _friendshipManager;
        internal readonly IWatchedMoviesManager _watchedMoviesManager;
        #endregion

        #region Facade
        private FilmLoFacade _facade;
        internal FilmLoFacade facadeYearStats => _facade ?? (_facade = new FilmLoFacade(_yearStatisticManager));
        internal FilmLoFacade facadeNotification => _facade ?? (_facade = new FilmLoFacade(_notificationManager));
        internal FilmLoFacade facadeWatchedMoviesStats => _facade ?? (_facade = new FilmLoFacade(_watchedMoviesStatsManager));
        internal FilmLoFacade facadePopularMovies => _facade ?? (_facade = new FilmLoFacade(_popularMoviesManager));
        internal FilmLoFacade facadeSavedMovies => _facade ?? (_facade = new FilmLoFacade(_savedMoviesManager));
        internal FilmLoFacade facadeUser => _facade ?? (_facade = new FilmLoFacade(_userManager, _friendshipManager));
        internal FilmLoFacade facadeWatchedMovies => _facade ?? (_facade = new FilmLoFacade(_watchedMoviesManager));
        #endregion

        #region Constructors
        //user contoller
        public BaseController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker,
            IUserManager userManager, IFriendshipManager friendshipManager)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
            _servicePropertyChecker = checker ??
                throw new ArgumentNullException(nameof(checker));
            _userManager = userManager ??
                 throw new ArgumentNullException(nameof(userManager));
            _friendshipManager = friendshipManager ??
                 throw new ArgumentNullException(nameof(friendshipManager));
        }

        // watched movies controller
        public BaseController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker,
            IYearStatisticManager yearStatisticManager, IWatchedMoviesStatsManager watchedMoviesStatsManager,
            IPopularMoviesManager popularMoviesManager, IWatchedMoviesManager watchedMoviesManager)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
            _servicePropertyChecker = checker ??
                throw new ArgumentNullException(nameof(checker));
            _yearStatisticManager = yearStatisticManager ??
                throw new ArgumentNullException(nameof(yearStatisticManager));
            _watchedMoviesStatsManager = watchedMoviesStatsManager ??
                throw new ArgumentNullException(nameof(watchedMoviesStatsManager));
            _popularMoviesManager = popularMoviesManager ??
                 throw new ArgumentNullException(nameof(popularMoviesManager));
            _watchedMoviesManager = watchedMoviesManager ??
                 throw new ArgumentNullException(nameof(watchedMoviesManager));

        }

        // Notification controller
        public BaseController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker, INotificationManager notificationManager)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
            _servicePropertyChecker = checker ??
                throw new ArgumentNullException(nameof(checker));
            _notificationManager = notificationManager ??
                throw new ArgumentNullException(nameof(notificationManager));
        }

        // Saved movies  controller
        public BaseController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker,
            ISavedMoviesManager savedMoviesManager)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
            _servicePropertyChecker = checker ??
                throw new ArgumentNullException(nameof(checker));
            _savedMoviesManager = savedMoviesManager ??
                throw new ArgumentNullException(nameof(savedMoviesManager));
        }
        #endregion

    }
}
