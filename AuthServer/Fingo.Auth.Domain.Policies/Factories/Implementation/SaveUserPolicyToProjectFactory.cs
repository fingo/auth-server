using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class SaveUserPolicyToProjectFactory:ISaveUserPolicyToProjectFactory
    {
        private readonly IProjectRepository projectRepository;
        private readonly IPolicyJsonConvertService policyJsonConvertService;

        public SaveUserPolicyToProjectFactory(IProjectRepository projectRepository, IPolicyJsonConvertService policyJsonConvertService)
        {
            this.projectRepository = projectRepository;
            this.policyJsonConvertService = policyJsonConvertService;
        }
        public ISaveUserPolicyToProject Create()
        {
            return new SaveUserPolicyToProject(projectRepository, policyJsonConvertService);
        }
    }
}