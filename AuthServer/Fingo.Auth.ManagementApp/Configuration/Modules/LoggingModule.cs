using Autofac;

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
            builder.RegisterGeneric(typeof(Infrastructure.Logging.Logger<>)).As(typeof(Infrastructure.Logging.ILogger<>)).InstancePerDependency();
        }
    }
}