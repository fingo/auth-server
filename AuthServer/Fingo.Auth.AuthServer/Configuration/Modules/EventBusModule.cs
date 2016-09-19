using Autofac;
using Fingo.Auth.Domain.Infrastructure.EventBus.Implementation;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.AuthServer.Configuration.Modules
{
    public class EventBusModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSubServices(builder);
        }

        private void RegisterSubServices(ContainerBuilder builder)
        {
            builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();

            builder.RegisterType<EventWatcher>().As<IEventWatcher>().InstancePerDependency();
        }
    }
}