using Autofac;
using Fingo.Auth.AuthServer.Services.Implementation;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Implementation;
using Fingo.Auth.Domain.Policies.Services.Interfaces;
using Fingo.Auth.Domain.Users.Services.Implementation;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.AuthServer.Configuration.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSubServices(builder);
        }

        private void RegisterSubServices(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerDependency();
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerDependency();
            builder.RegisterType<JwtLibraryWrapperService>().As<IJwtLibraryWrapperService>().InstancePerDependency();
            builder.RegisterType<PolicyJsonConvertService>().As<IPolicyJsonConvertService>().InstancePerDependency();
            builder.RegisterType<HashingService>().As<IHashingService>().InstancePerDependency();
            builder.RegisterType<CustomDataJsonConvertService>()
                .As<ICustomDataJsonConvertService>()
                .InstancePerDependency();
            builder.RegisterType<SetDefaultUserCustomDataBasedOnProject>().As<ISetDefaultUserCustomDataBasedOnProject>();
        }
    }
}