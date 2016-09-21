using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class AddProjectCustomDataToProjectFactory : IAddProjectCustomDataToProjectFactory
    {
        private readonly IEventBus _eventBus;
        private readonly ICustomDataJsonConvertService _jsonConvertService;
        private readonly IProjectRepository _projectRepository;

        public AddProjectCustomDataToProjectFactory(IProjectRepository projectRepository ,
            ICustomDataJsonConvertService jsonConvertService , IEventBus eventBus)
        {
            _eventBus = eventBus;
            _jsonConvertService = jsonConvertService;
            _projectRepository = projectRepository;
        }

        public IAddProjectCustomDataToProject Create()
        {
            return new AddProjectCustomDataToProject(_projectRepository , _jsonConvertService , _eventBus);
        }
    }
}