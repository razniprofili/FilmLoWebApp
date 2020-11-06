using Common.Exceptions;
using FilmLoApp.API.Controllers;
using FilmLoApp.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;
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
                throw new AuthenticationException("Nema autorizacionog hedera!");

            string token = authheder.Value.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(token))
                throw new AuthenticationException("Autorizacioni heder ne sme biti prazan!");

            UserJwtModel user;
            try
            {
                //dekodiranje generisanog tokena
                user = SecurityHelper.Decode<UserJwtModel>(token);

            }
            catch
            {
                throw new AuthenticationException("Token nije validan!");
            }

            if (user.ExpirationTime < DateTime.UtcNow)
                throw new AuthenticationException("Token je istekao, ponovo se ulogujte!");

            //if (Roles != null && !Roles.Split(',').ToList().Contains(user.Role.Name)) //ako u listi dozvoljenih rola ne postoji rola ulogovanog usera
            //    throw new AuthenticationException("Nemate prava pristupa!");

            var controller = context.Controller as BaseController;

            if (controller != null)
                controller.CurrentUser = user;

            base.OnActionExecuting(context);
        }
    }
}
