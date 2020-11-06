using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Helpers
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        //ako se posalju username i password i ako se oni ne poklapaju sa modelom
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(context.ModelState); //vraca 400, Bad request        
        }
    }
}
