using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class GetCustomDataConfigurationOrDefaultForProjectFactory :
        IGetCustomDataConfigurationOrDefaultForProjectFactory
    {
        private readonly ICustomDataJsonConvertService _jsonConvertService;
        private readonly IProjectRepository _projectRepository;

        public GetCustomDataConfigurationOrDefaultForProjectFactory(IProjectRepository projectRepository ,
            ICustomDataJsonConvertService jsonConvertService)
        {
            _projectRepository = projectRepository;
            _jsonConvertService = jsonConvertService;
        }

        public IGetCustomDataConfigurationOrDefaultForProject Create()
        {
            return new GetCustomDataConfigurationOrDefaultForProject(_projectRepository , _jsonConvertService);
        }
    }
}