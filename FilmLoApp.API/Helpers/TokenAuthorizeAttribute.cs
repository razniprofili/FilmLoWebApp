using Common.Exceptions;
using FilmLoApp.API.Controllers;
//using FilmLoApp.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Helpers
{
    public class TokenAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authheder = context.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "Authorization");

            if (authheder.Key == null)
                throw new AuthenticationException("No authorization header!");

            string token = authheder.Value.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(token))
                throw new AuthenticationException("Authorization header must not be empty!");

            UserJwtModel user;
            try
            {
                //dekodiranje generisanog tokena
                user = SecurityHelper.Decode<UserJwtModel>(token);

            }
            catch
            {
                throw new AuthenticationException("Token not valid!");
            }

            if (user.ExpirationTime < DateTime.Now)
                throw new AuthenticationException("Token expired, login again!");

            var controller = context.Controller as BaseController;

            if (controller != null)
                controller.CurrentUser = user;

            base.OnActionExecuting(context);
        }
    }
}
