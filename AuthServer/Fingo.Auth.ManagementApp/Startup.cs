using System;
using Autofac;
using Fingo.Auth.AuthServer.Client.Services.Implementation;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.DbAccess.Context;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.ManagementApp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Fingo.Auth.ManagementApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json" , true , true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json" , true , true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();

            IPostService postService = new PostService();
            IRemoteTokenService remoteTokenService = new RemoteTokenService(postService);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConfiguration.PolicyName , policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == AuthorizationConfiguration.PolicyName) &&
                            remoteTokenService.VerifyToken(c.Value))));
            });

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc();

            // Database
            services.AddDbContext<AuthServerContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            return DependencyInjectionConfigurator.ConfigureContainer(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app , IHostingEnvironment env , ILoggerFactory loggerFactory ,
            IAuthServerContext dbContext)
        {
            app.UseSession();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookie" ,
                LoginPath = new PathString("/Account/LoginPage/") ,
                AccessDeniedPath = new PathString("/Account/LoginPage/") ,
                AutomaticAuthenticate = true ,
                AutomaticChallenge = true
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("DefaultRest" ,
                    "{controller}/{id?}");

                routes.MapRoute(
                    "default" ,
                    "{controller=Account}/{action=LoginPage}/{id?}");
            });

            // configuring logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile("C:\\CSharpInternship16DontDelete\\Logs\\managementapp-{Date}.log" ,
                    outputTemplate :
                    "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
                .Enrich.WithThreadId()
                .CreateLogger();

            // apply pending migrations, log exception and throw exception to prevent application from starting

            try
            {
                dbContext.PerformMigration();
            }
            catch (Exception ex)
            {
                Log.Error($"<Startup> Unable to PerformMigration(), exception stack trace: {ex.StackTrace}");
                throw;
            }

            SetAuthServerClientConfiguration();
            SetManagementAppEmailConfiguration();
        }

        private void SetAuthServerClientConfiguration()
        {
            AuthServer.Client.Configuration.CreateNewUserInProjectAdress =
                Configuration["Data:ApiAdresses:CreateNewUserInProject"];
            AuthServer.Client.Configuration.VerifyTokenAdress = Configuration["Data:ApiAdresses:VerifyToken"];
            AuthServer.Client.Configuration.AcquireTokenAdress = Configuration["Data:ApiAdresses:AcquireToken"];
            AuthServer.Client.Configuration.ChangingPasswordUserAddress =
                Configuration["Data:ApiAdresses:PasswordChangeForUser"];
            AuthServer.Client.Configuration.Guid = Configuration["Data:ManagementAppGuid"];
            AuthServer.Client.Configuration.SetPasswordForUserAdress =
                Configuration["Data:ApiAdresses:SetPasswordForUser"];
        }

        private void SetManagementAppEmailConfiguration()
        {
            EmailConfiguration.ActivateEmail = Configuration["Data:ApiAdresses:ActivateEmail"];
            EmailConfiguration.ServerEmail = Configuration["Data:ConfigurationToSendEmail:ServerEmail"];
            EmailConfiguration.ServerName = Configuration["Data:ConfigurationToSendEmail:ServerName"];
            EmailConfiguration.ServerPassword = Configuration["Data:ConfigurationToSendEmail:ServerPassword"];
            EmailConfiguration.Content = Configuration["Data:ConfigurationToSendEmail:Content"];
            EmailConfiguration.Greeting = Configuration["Data:ConfigurationToSendEmail:Greeting"];
            EmailConfiguration.Sender = Configuration["Data:ConfigurationToSendEmail:Sender"];
            EmailConfiguration.SenderEmail = Configuration["Data:ConfigurationToSendEmail:SenderEmail"];
            EmailConfiguration.EmailTitle = Configuration["Data:ConfigurationToSendEmail:EmailTitle"];
            EmailConfiguration.GrantRole = Configuration["Data:ConfigurationToSendEmail:GrantRole"];
            EmailConfiguration.RevokeRole = Configuration["Data:ConfigurationToSendEmail:RevokeRole"];
            EmailConfiguration.EmailGrantTitle = Configuration["Data:ConfigurationToSendEmail:EmailGrantTitle"];
            EmailConfiguration.NewAccountCreated = Configuration["Data:ConfigurationToSendEmail:NewAccountCreated"];
            EmailConfiguration.ContentSetPassword = Configuration["Data:ConfigurationToSendEmail:ContentSetPassword"];
        }
    }
}