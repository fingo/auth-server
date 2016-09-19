using System;
using Fingo.Auth.AuthServer.Configuration;
using Fingo.Auth.DbAccess.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Fingo.Auth.AuthServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json" , optional: true , reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json" , optional: true , reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDataProtection();

            services.AddDbContext<AuthServerContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            return DependencyInjectionConfigurator.ConfigureContainer(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // configuring logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile("C:\\CSharpInternship16DontDelete\\Logs\\authserver-{Date}.log", outputTemplate:
                    "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
                .Enrich.WithThreadId()
                .CreateLogger();

            SetConfiguration();
        }

        private void SetConfiguration()
        {
            RedirectConfiguration.PasswordExpiredRedirectAdress = Configuration["Data:PasswordExpiredRedirectAdress"];
            JwtConfiguration.SecretKey = Configuration["Data:JwtSecretKey"];
        }
    }
}
