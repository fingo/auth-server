using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class SavePolicyToProjectFactory : ISavePolicyToProjectFactory
    {
        private readonly IEventBus _eventBus;
        private readonly IPolicyJsonConvertService _jsonConvertService;
        private readonly IProjectRepository _projectRepository;

        public SavePolicyToProjectFactory(IPolicyJsonConvertService jsonConvertService , IEventBus eventBus ,
            IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _eventBus = eventBus;
            _jsonConvertService = jsonConvertService;
        }

        public ISavePolicyToProject Create()
        {
            return new SavePolicyToProject(_projectRepository , _eventBus , _jsonConvertService);
        }
    }
}