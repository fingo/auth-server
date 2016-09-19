using Autofac;
using Fingo.Auth.Infrastructure.Logging;

namespace Fingo.Auth.AuthServer.Configuration.Modules
{
    public class LoggingModule : Autofac.Module
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