using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.UserView;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class GetUserCustomDataConfigurationView : IGetUserCustomDataConfigurationView
    {
        private readonly IProjectRepository projectRepository;
        private readonly ICustomDataJsonConvertService jsonConvertService;
        private readonly IUserCustomDataRepository userCustomDataRepository;

        public GetUserCustomDataConfigurationView(IProjectRepository projectRepository,
            ICustomDataJsonConvertService jsonConvertService, IUserCustomDataRepository userCustomDataRepository)
        {
            this.projectRepository = projectRepository;
            this.jsonConvertService = jsonConvertService;
            this.userCustomDataRepository = userCustomDataRepository;
        }

        public UserConfigurationView Invoke(int projectId, int userId, ConfigurationType configurationType,
            string configurationName)
        {
            var project = projectRepository.GetByIdWithCustomDatas(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var userCustomConfiguration = userCustomDataRepository.GetUserCustomData(projectId , userId ,
                configurationName);
            if (userCustomConfiguration == null)
                throw new ArgumentNullException(
                    $"Could not find user configuration data connected with projectId: {projectId} and userId{userId}.");

            switch (configurationType)
            {
                case ConfigurationType.Boolean:
                    {
                        BooleanUserConfiguration userConfiguration = (BooleanUserConfiguration)
                            jsonConvertService.DeserializeUser(ConfigurationType.Boolean,
                                userCustomConfiguration.SerializedConfiguration);

                        return new BooleanUserConfigurationView()
                        {
                            ProjectId = projectId,
                            UserId = userId,
                            ConfigurationName = configurationName,
                            CurrentValue = userConfiguration.Value
                        };
                    }
                case ConfigurationType.Number:
                    {
                        NumberProjectConfiguration projectConfiguration =
                            (NumberProjectConfiguration)
                            jsonConvertService.DeserializeProject(ConfigurationType.Number,
                                project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName)
                                    .SerializedConfiguration);
                        NumberUserConfiguration userConfiguration =
                            (NumberUserConfiguration)
                            jsonConvertService.DeserializeUser(ConfigurationType.Number ,
                                userCustomConfiguration.SerializedConfiguration);

                        return new NumberUserConfigurationView()
                        {
                            ProjectId = projectId,
                            UserId = userId,
                            ConfigurationName = configurationName,
                            CurrentValue = userConfiguration.Value,
                            LowerBound = projectConfiguration.LowerBound,
                            UpperBound = projectConfiguration.UpperBound
                        };
                    }
                case ConfigurationType.Text:
                {
                    TextProjectConfiguration projectConfiguration =
                        (TextProjectConfiguration) jsonConvertService.DeserializeProject(ConfigurationType.Text ,
                            project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName && m.ProjectId==projectId)
                                .SerializedConfiguration);

                    TextUserConfiguration userConfiguration =
                        (TextUserConfiguration)
                        jsonConvertService.DeserializeUser(ConfigurationType.Text ,
                            userCustomConfiguration.SerializedConfiguration);

                    var possibleValues = new List<string>();

                    if (!projectConfiguration.PossibleValues.Contains(projectConfiguration.Default))
                        possibleValues.Add(projectConfiguration.Default);

                    possibleValues.AddRange(projectConfiguration.PossibleValues);

                    return new TextUserConfigurationView()
                    {
                        ProjectId = projectId ,
                        UserId = userId ,
                        ConfigurationName = configurationName ,
                        CurrentValue = userConfiguration.Value ,
                        PossibleValuesList = possibleValues
                    };
                }

            }

            throw new Exception($"Something went wrong in GetUserConfigurationViewForUser");
        }
    }
}