using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fingo.Auth.ManagementApp.Configuration.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Fingo.Auth.ManagementApp.Configuration
{
    public static class DependencyInjectionConfigurator
    {
        public static IServiceProvider ConfigureContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            RegisterModules(builder);

            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        private static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule(new DbModule());
            builder.RegisterModule(new LoggingModule());
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new DomainFactoryModule());
            builder.RegisterModule(new EventBusModule());
        }
    }
}