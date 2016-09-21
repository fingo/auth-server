using Autofac;
using Fingo.Auth.AuthServer.Client.Services.Implementation;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Implementation;
using Fingo.Auth.Domain.Policies.Services.Interfaces;
using Fingo.Auth.Domain.Users.Services.Implementation;
using Fingo.Auth.Domain.Users.Services.Interfaces;
using Fingo.Auth.ManagementApp.Services.Implementation;
using Fingo.Auth.ManagementApp.Services.Interfaces;

namespace Fingo.Auth.ManagementApp.Configuration.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSubServices(builder);
        }

        private void RegisterSubServices(ContainerBuilder builder)
        {
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerDependency();
            builder.RegisterType<CsvService>().As<ICsvService>().InstancePerDependency();
            builder.RegisterType<SessionService>().As<ISessionService>().InstancePerDependency();
            builder.RegisterType<MessageSender>().As<IMessageSender>().InstancePerDependency();
            builder.RegisterType<RemoteTokenService>().As<IRemoteTokenService>().InstancePerDependency();
            builder.RegisterType<RemoteAccountService>().As<IRemoteAccountService>().InstancePerDependency();
            builder.RegisterType<PostService>().As<IPostService>().InstancePerDependency();
            builder.RegisterType<PolicyJsonConvertService>().As<IPolicyJsonConvertService>().InstancePerDependency();
            builder.RegisterType<CustomDataJsonConvertService>()
                .As<ICustomDataJsonConvertService>()
                .InstancePerDependency();
            builder.RegisterType<SetDefaultUserCustomDataBasedOnProject>().As<ISetDefaultUserCustomDataBasedOnProject>();
        }
    }
}