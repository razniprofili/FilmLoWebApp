
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facade;
using Models;

namespace FilmLoApp.API.Controllers
{
    public class BaseController : Controller
    {
        
        internal UserJwtModel CurrentUser { get; set; }


        private FilmLoFacade _facade;
        internal FilmLoFacade facade => _facade ?? (_facade = new FilmLoFacade());


    }
}
