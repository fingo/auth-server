using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class SaveUserCustomDataFactory : ISaveUserCustomDataFactory
    {
        private readonly ICustomDataJsonConvertService customDataJsonConvertService;
        private readonly IUserCustomDataRepository userCustomDataRepository;

        public SaveUserCustomDataFactory(IUserCustomDataRepository userCustomDataRepository ,
            ICustomDataJsonConvertService customDataJsonConvertService)
        {
            this.userCustomDataRepository = userCustomDataRepository;
            this.customDataJsonConvertService = customDataJsonConvertService;
        }

        public ISaveUserCustomData Create()
        {
            return new SaveUserCustomData(userCustomDataRepository , customDataJsonConvertService);
        }
    }
}