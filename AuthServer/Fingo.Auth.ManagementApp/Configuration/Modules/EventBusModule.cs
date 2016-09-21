using Autofac;
using Fingo.Auth.Domain.Infrastructure.EventBus.Implementation;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.ManagementApp.Configuration.Modules
{
    public class EventBusModule : Module
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