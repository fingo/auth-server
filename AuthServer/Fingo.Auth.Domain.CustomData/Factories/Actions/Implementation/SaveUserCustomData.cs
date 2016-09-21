using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class SaveUserCustomData : ISaveUserCustomData
    {
        private readonly ICustomDataJsonConvertService customDataJsonConvertService;
        private readonly IUserCustomDataRepository userCustomDataRepository;

        public SaveUserCustomData(IUserCustomDataRepository userCustomDataRepository ,
            ICustomDataJsonConvertService customDataJsonConvertService)
        {
            this.userCustomDataRepository = userCustomDataRepository;
            this.customDataJsonConvertService = customDataJsonConvertService;
        }

        public void Invoke(int projectId , int userId , string name , UserConfiguration configuration)
        {
            var userCustomData =
                userCustomDataRepository.FindBy(
                    m =>
                        (m.ProjectCustomData.ProjectId == projectId) && (m.UserId == userId) &&
                        (m.ProjectCustomData.ConfigurationName == name)).FirstOrDefault() ?? new UserCustomData();
            userCustomData.SerializedConfiguration = customDataJsonConvertService.Serialize(configuration);
            userCustomData.ModificationDate = DateTime.UtcNow;

            userCustomDataRepository.Edit(userCustomData);
        }
    }
}