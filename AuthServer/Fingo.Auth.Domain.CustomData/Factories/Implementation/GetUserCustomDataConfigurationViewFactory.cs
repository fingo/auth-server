using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class GetUserCustomDataConfigurationViewFactory : IGetUserCustomDataConfigurationViewFactory
    {
        private readonly IProjectRepository projectRepository;
        private readonly ICustomDataJsonConvertService jsonConvertService;
        private readonly IUserCustomDataRepository userCustomDataRepository;

        public GetUserCustomDataConfigurationViewFactory(IProjectRepository projectRepository,
            ICustomDataJsonConvertService jsonConvertService, IUserCustomDataRepository userCustomDataRepository)
        {
            this.projectRepository = projectRepository;
            this.jsonConvertService = jsonConvertService;
            this.userCustomDataRepository = userCustomDataRepository;
        }

        public IGetUserCustomDataConfigurationView Create()
        {
            return new GetUserCustomDataConfigurationView(projectRepository, jsonConvertService, userCustomDataRepository);
        }
    }
}