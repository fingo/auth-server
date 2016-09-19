using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class GetPolicyConfigurationOrDefaultFromProjectFactory : IGetPolicyConfigurationOrDefaultFromProjectFactory
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPolicyJsonConvertService _jsonConvertService;

        public GetPolicyConfigurationOrDefaultFromProjectFactory(IProjectRepository projectRepository,
            IPolicyJsonConvertService jsonConvertService)
        {
            _projectRepository = projectRepository;
            _jsonConvertService = jsonConvertService;
        }

        public IGetPolicyConfigurationOrDefaultFromProject Create()
        {
            return new GetPolicyConfigurationOrDefaultFromProject(_projectRepository, _jsonConvertService);
        }
    }
}
