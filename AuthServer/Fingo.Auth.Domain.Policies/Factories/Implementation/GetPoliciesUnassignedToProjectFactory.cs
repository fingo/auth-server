using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class GetPoliciesUnassignedToProjectFactory : IGetPoliciesUnassignedToProjectFactory
    {
        private readonly IProjectRepository _projectRepository;

        public GetPoliciesUnassignedToProjectFactory(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IGetPoliciesUnassignedToProject Create()
        {
            return new GetPoliciesUnassignedToProject(_projectRepository);
        }
    }
}