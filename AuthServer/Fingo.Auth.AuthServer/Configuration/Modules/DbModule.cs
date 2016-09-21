using Autofac;
using Fingo.Auth.DbAccess.Context;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Repository.Implementation;
using Fingo.Auth.DbAccess.Repository.Interfaces;

namespace Fingo.Auth.AuthServer.Configuration.Modules
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSubServices(builder);
        }

        private void RegisterSubServices(ContainerBuilder builder)
        {
            builder.RegisterType<AuthServerContext>().As<IAuthServerContext>().InstancePerDependency();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerDependency();
            builder.RegisterType<AuditLogRepository>().As<IAuditLogRepository>().InstancePerDependency();
        }
    }
}