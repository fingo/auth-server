using Autofac;
using Fingo.Auth.Infrastructure.Logging;

namespace Fingo.Auth.ManagementApp.Configuration.Modules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSubServices(builder);
        }

        private void RegisterSubServices(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).InstancePerDependency();
        }
    }
}