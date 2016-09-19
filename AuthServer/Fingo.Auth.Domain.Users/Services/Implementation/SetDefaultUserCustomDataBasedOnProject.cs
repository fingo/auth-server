using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Services.Implementation
{
    public class SetDefaultUserCustomDataBasedOnProject:ISetDefaultUserCustomDataBasedOnProject
    {
        private readonly ICustomDataJsonConvertService customDataJsonConvertService;

        public SetDefaultUserCustomDataBasedOnProject(ICustomDataJsonConvertService customDataJsonConvertService)
        {
            this.customDataJsonConvertService = customDataJsonConvertService;
        }

        public void SetDefaultUserCustomData(Project project, User newUser)
        {
            var projectCustomData = project.ProjectCustomData;
            foreach (var customData in projectCustomData)
            {
                switch (customData.ConfigurationType)
                {
                    case ConfigurationType.Boolean:
                    {
                        var projectConfiguration =
                            (BooleanProjectConfiguration)
                            customDataJsonConvertService.DeserializeProject(ConfigurationType.Boolean ,
                                customData.SerializedConfiguration);
                        newUser.UserCustomData.Add(new UserCustomData()
                        {
                            ProjectCustomDataId = customData.Id,
                            SerializedConfiguration =
                                customDataJsonConvertService.Serialize(new BooleanUserConfiguration()
                                {
                                    Value = projectConfiguration.Default
                                })
                        });
                        break;
                    }
                    case ConfigurationType.Number:
                    {
                        var projectConfiguration =
                            (NumberProjectConfiguration)
                            customDataJsonConvertService.DeserializeProject(ConfigurationType.Number ,
                                customData.SerializedConfiguration);
                        newUser.UserCustomData.Add(new UserCustomData()
                        {
                            ProjectCustomDataId = customData.Id,
                            SerializedConfiguration =
                                customDataJsonConvertService.Serialize(new NumberUserConfiguration()
                                {
                                    Value = projectConfiguration.Default
                                })
                        });
                        break;
                    }
                    case ConfigurationType.Text:
                    {
                        var projectConfiguration =
                            (TextProjectConfiguration)
                            customDataJsonConvertService.DeserializeProject(ConfigurationType.Text ,
                                customData.SerializedConfiguration);

                        newUser.UserCustomData.Add(new UserCustomData()
                        {
                            ProjectCustomDataId = customData.Id,
                            SerializedConfiguration =
                                customDataJsonConvertService.Serialize(new TextUserConfiguration()
                                {
                                    Value = projectConfiguration.Default
                                })
                        });
                        break;
                    }
                }
            }
        }
    }
}