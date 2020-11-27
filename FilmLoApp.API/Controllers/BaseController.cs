
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facade;
using Models;
using AutoMapper;
using Core.Services;

namespace FilmLoApp.API.Controllers
{
    public class BaseController : Controller
    {
        
        internal UserJwtModel CurrentUser { get; set; }


        private FilmLoFacade _facade;
        internal FilmLoFacade facade => _facade ?? (_facade = new FilmLoFacade());

        internal readonly IMapper _mapper;

        internal readonly IPropertyMappingService _service;

        internal readonly IPropertyCheckerService _servicePropertyChecker;

        public BaseController(IMapper mapper, IPropertyMappingService service, IPropertyCheckerService checker)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
            _servicePropertyChecker = checker ??
                throw new ArgumentNullException(nameof(checker));
        }


    }
}
