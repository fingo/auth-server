using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class GetUserCustomDataListFromProjectFactory : IGetUserCustomDataListFromProjectFactory
    {
        private readonly ICustomDataJsonConvertService _convertService;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public GetUserCustomDataListFromProjectFactory(IProjectRepository projectRepository ,
            IUserRepository userRepository , ICustomDataJsonConvertService convertService)
        {
            _convertService = convertService;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public IGetUserCustomDataListFromProject Create()
        {
            return new GetUserCustomDataListFromProject(_projectRepository , _userRepository , _convertService);
        }
    }
}