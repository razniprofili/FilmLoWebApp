using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
//using AutoMapper.Configuration;
using Common.Helpers;
using Core.Services;
using Data;
using Domain;
using FilmLoApp.API.Helpers;
using FilmLoApp.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace FilmLoApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Helper.ConnectionString = Configuration.GetConnectionString("DemoDatabase");
            SecurityHelper.SecretKey = Configuration.GetValue<string>("SecretKey");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // za kesiranje
            //services.AddHttpCacheHeaders((expirationModelOptions) =>
            //{
            //    expirationModelOptions.MaxAge = 60;
            //    expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            //},
            //(validationModelOptions) =>
            //{
            //    validationModelOptions.MustRevalidate = true;
            //});

            // services.AddResponseCaching();

            //services.AddControllers(setupAction =>
            //{
            //    setupAction.ReturnHttpNotAcceptable = true; //ako npr xml nije podrzavajuc on ce vratiti gresku 406, a ne u JSON formatu koji je default
            //    //za kesiranje, da bi se primelo isto pravilo nad razl resursima
            //    setupAction.CacheProfiles.Add("240SecondsCacheProfile",
            //                                    new CacheProfile()
            //                                    {
            //                                        Duration = 240
            //                                    });
            //});
          //  services.AddResponseCompression();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            //services.AddControllers(setupAction =>
            //{
            //    setupAction.ReturnHttpNotAcceptable = true; //ako npr xml nije podrzavajuc on ce vratiti gresku 406, a ne u JSON formatu koji je default
            //    //za kesiranje, da bi se primelo isto pravilo nad razl resursima
            //    setupAction.CacheProfiles.Add("240SecondsCacheProfile",
            //                                    new CacheProfile()
            //                                    {
            //                                        Duration = 240
            //                                    });
            //})//dodajemo ovo da bi mogao da cita JSON patch doc kod partial updejtovanja
            ////dodati pre xla da se ne bi promenio defaltni tip
            //.AddNewtonsoftJson(setupAction =>
            //{
            //    setupAction.SerializerSettings.ContractResolver =
            //       new CamelCasePropertyNamesContractResolver();
            //})
            //.AddXmlDataContractSerializerFormatters().ConfigureApiBehaviorOptions(setupAction =>
            //{
            //    setupAction.InvalidModelStateResponseFactory = context =>
            //    {
            //        // create a problem details object
            //        var problemDetailsFactory = context.HttpContext.RequestServices
            //            .GetRequiredService<ProblemDetailsFactory>();
            //        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
            //                context.HttpContext,
            //                context.ModelState);

            //        // add additional info not added by default
            //        problemDetails.Detail = "See the errors field for details.";
            //        problemDetails.Instance = context.HttpContext.Request.Path;

            //        // find out which status code to use
            //        var actionExecutingContext =
            //              context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

            //        // if there are modelstate errors & all keys were correctly
            //        // found/parsed we're dealing with validation errors
            //        if ((context.ModelState.ErrorCount > 0) &&
            //            (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
            //        {
            //            problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
            //            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            //            problemDetails.Title = "One or more validation errors occurred.";

            //            return new UnprocessableEntityObjectResult(problemDetails)
            //            {
            //                ContentTypes = { "application/problem+json" }
            //            };
            //        }

            //        // if one of the keys wasn't correctly found / couldn't be parsed
            //        // we're dealing with null/unparsable input
            //        problemDetails.Status = StatusCodes.Status400BadRequest;
            //        problemDetails.Title = "One or more errors on input occurred.";
            //        return new BadRequestObjectResult(problemDetails)
            //        {
            //            ContentTypes = { "application/problem+json" }
            //        };
            //    };
            //}
            //);

            services.AddCors();
            //services.AddScoped<DbContext, FilmLoWebAppContext>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            //6. lekcija, da bi podrzao prikaz sa i bez hateoas-a
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                      .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoas+json");
                }
            });

            // register PropertyMappingService
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            // register PropertyCheckerService
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
      

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // app.UseCors(options => options.WithOrigins("http://localhost:8100"));

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()

            );
            //za kesiranje:
            // app.UseResponseCaching();

            // app.UseHttpCacheHeaders(); //mora biti na ovom mestu
           // app.UseResponseCompression();
            app.UseMvc();

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
