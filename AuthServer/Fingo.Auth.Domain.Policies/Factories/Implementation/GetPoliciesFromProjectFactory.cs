using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class GetPoliciesFromProjectFactory : IGetPoliciesFromProjectFactory
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPolicyJsonConvertService _jsonConvertService;

        public GetPoliciesFromProjectFactory(IProjectRepository projectRepository, IPolicyJsonConvertService jsonConvertService)
        {
            _projectRepository = projectRepository;
            _jsonConvertService = jsonConvertService;
        }

        public IGetPoliciesFromProject Create()
        {
            return new GetPoliciesFromProject(_projectRepository, _jsonConvertService);
        }
    }
}
