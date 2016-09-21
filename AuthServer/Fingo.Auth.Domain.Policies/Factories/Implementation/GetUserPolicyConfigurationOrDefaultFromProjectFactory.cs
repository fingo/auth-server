using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class GetUserPolicyConfigurationOrDefaultFromProjectFactory :
        IGetUserPolicyConfigurationOrDefaultFromProjectFactory
    {
        private readonly IPolicyJsonConvertService policyJsonConvertService;
        private readonly IUserRepository userRepository;

        public GetUserPolicyConfigurationOrDefaultFromProjectFactory(IUserRepository userRepository ,
            IPolicyJsonConvertService policyJsonConvertService)
        {
            this.userRepository = userRepository;
            this.policyJsonConvertService = policyJsonConvertService;
        }

        public IGetUserPolicyConfigurationOrDefaultFromProject Create()
        {
            return new GetUserPolicyConfigurationOrDefaultFromProject(userRepository , policyJsonConvertService);
        }
    }
}