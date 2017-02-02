using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Model.Core;
using Model.Models;
using Swashbuckle.Swagger.Model;

namespace LearnOurLanguage.Web
{
    /// <summary>
    /// Klasa startowa, zawierająca konfiguracje bazowych elementów
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Kontener, fabryka do wstrzykiwania zależności (DI)
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// Konfiguracja aplikacji
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Konstruktor ładujący pliki konfiguracyjne
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Konfiguracja serwisów użytych w aplikacji
        /// </summary>
        /// <param name="services">Kolekcja serwisów</param>
        /// <returns>Service Provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // api documentation
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Learn our Language - WebAPI",
                    Description = "",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
            });

            // Add framework services.
            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    options.CacheProfiles.Add("NoCache", new CacheProfile
                    {
                        NoStore = true,
                        Duration = 0
                    });
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Database DI
            var connectionString = Configuration.GetConnectionString("LearnOurLanguageContext");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".LearnOurLanguage";
            });

            // Autofac
            var builder = new ContainerBuilder();
            ConfigHelper.BindAssemblyForBuilder(ref builder, "LearnOurLanguage.Web", "Model");
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        /// <summary>
        /// Metoda konfigurująca parametry aplikacji
        /// </summary>
        /// <param name="app"></param>
        /// <param name="appLifetime"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/log-{Date}.txt");

            App.LoggerFactory = loggerFactory;
            App.Logger = loggerFactory.CreateLogger("trace");
            
            app.UseSession();

            app.UseHttpException();
            app.UseDeveloperExceptionPage();

            app.UseMvc();
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Response.StatusCode = 200;
                    context.Request.Path = "/";
                    await next();
                }
            });

            app.UseFileServer();

            string libPath = Path.GetFullPath(Path.Combine(env.WebRootPath, @"..\node_modules\"));
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(libPath),
                RequestPath = new PathString("/node_modules"),
                EnableDirectoryBrowsing = false
            });
            
            app.UseSwagger();
            app.UseSwaggerUi();

            appLifetime.ApplicationStopped.Register(() =>
            {
                this.ApplicationContainer.Dispose();
            });
        }
    }
}
