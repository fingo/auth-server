using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class GetUserDataViewFactory: IGetUserDataViewFactory
    {
        private readonly IUserRepository userRepository;
        private readonly IPolicyJsonConvertService jsonConvertService;

        public GetUserDataViewFactory(IUserRepository userRepository, IPolicyJsonConvertService jsonConvertService)
        {
            this.userRepository = userRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public IGetUserDataView Create()
        {
            return new GetUserDataView(userRepository, jsonConvertService);
        }
    }
}