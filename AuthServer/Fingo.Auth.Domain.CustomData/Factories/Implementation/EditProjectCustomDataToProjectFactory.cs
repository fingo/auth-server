using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class EditProjectCustomDataToProjectFactory : IEditProjectCustomDataToProjectFactory
    {
        private readonly ICustomDataJsonConvertService _jsonConvertService;
        private readonly IProjectRepository _projectRepository;

        public EditProjectCustomDataToProjectFactory(IProjectRepository projectRepository ,
            ICustomDataJsonConvertService jsonConvertService)
        {
            _jsonConvertService = jsonConvertService;
            _projectRepository = projectRepository;
        }

        public IEditProjectCustomDataToProject Create()
        {
            return new EditProjectCustomDataToProject(_projectRepository , _jsonConvertService);
        }
    }
}