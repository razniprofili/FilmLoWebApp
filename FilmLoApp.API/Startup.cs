using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Helpers;
using Core;
using Core.Services;
using Data;
using Domain;
using FilmLoApp.API.Helpers;
using FilmLoApp.API.Hubs;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddCors();

            //da bi podrzao prikaz sa i bez hateoas-a
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                      .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoas+json");
                }
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();
            services.AddScoped<IYearStatisticManager, YearStatisticManager>();
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddScoped<IWatchedMoviesStatsManager, WatchedMoviesStatsManager>();
            services.AddScoped<IPopularMoviesManager, PopularMoviesManager>();
            services.AddScoped<ISavedMoviesManager, SavedMoviesManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFriendshipManager, FriendshipManager>();
            services.AddScoped<IWatchedMoviesManager, WatchedMoviesManager>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddDbContext<FilmLoWebAppContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSignalR();
        }
      

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed((hosts) => true)
            );
            app.UseMvc();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SendFriendRequestHub>("/sendRequest");
                endpoints.MapHub<NotifyHub>("/sendNotification");
            });
        }
    }
}
