using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.CustomData;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class AddProjectCustomDataToProject : IAddProjectCustomDataToProject
    {
        private readonly IEventBus _eventBus;
        private readonly ICustomDataJsonConvertService _jsonConvertService;
        private readonly IProjectRepository _projectRepository;

        public AddProjectCustomDataToProject(IProjectRepository projectRepository ,
            ICustomDataJsonConvertService jsonConvertService , IEventBus eventBus)
        {
            _eventBus = eventBus;
            _jsonConvertService = jsonConvertService;
            _projectRepository = projectRepository;
        }

        public void Invoke(int projectId , string name , ConfigurationType type , ProjectConfiguration configuration)
        {
            var project = _projectRepository.GetByIdWithCustomDatas(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var customData = project.ProjectCustomData.FirstOrDefault(data => data.ConfigurationName == name);
            if (customData == null)
            {
                project.ProjectCustomData.Add(new ProjectCustomData
                {
                    ConfigurationName = name ,
                    ConfigurationType = type ,
                    SerializedConfiguration = _jsonConvertService.Serialize(configuration)
                });
                AddConfigurationForUsers(name , type , configuration , project);
            }
            else
                throw new Exception($"Custom data of name: {name} already exists.");

            _projectRepository.Edit(project);
            _eventBus.Publish(new CustomDataAdded(projectId , name , type));
        }

        private void AddConfigurationForUsers(string name , ConfigurationType type , ProjectConfiguration configuration ,
            Project project)
        {
            switch (type)
            {
                case ConfigurationType.Boolean:
                {
                    AddBooleanConfigurationToUsers((BooleanProjectConfiguration) configuration , project , name);
                    break;
                }
                case ConfigurationType.Number:
                {
                    AddNumberConfigurationToUsers((NumberProjectConfiguration) configuration , project , name);
                    break;
                }
                case ConfigurationType.Text:
                {
                    AddTextConfigurationToUsers((TextProjectConfiguration) configuration , project , name);
                    break;
                }
            }
        }

        private void AddBooleanConfigurationToUsers(BooleanProjectConfiguration configuration , Project project ,
            string configurationName)
        {
            foreach (var projectProjectUser in project.ProjectUsers)
            {
                var projectCustomData =
                    project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName);
                projectCustomData.UserCustomData.Add(new UserCustomData
                {
                    UserId = projectProjectUser.UserId ,
                    SerializedConfiguration =
                        _jsonConvertService.Serialize(new BooleanUserConfiguration {Value = configuration.Default})
                });
            }
        }

        private void AddNumberConfigurationToUsers(NumberProjectConfiguration configuration , Project project ,
            string configurationName)
        {
            foreach (var projectProjectUser in project.ProjectUsers)
            {
                var projectCustomData =
                    project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName);
                projectCustomData.UserCustomData.Add(new UserCustomData
                {
                    UserId = projectProjectUser.UserId ,
                    SerializedConfiguration =
                        _jsonConvertService.Serialize(new NumberUserConfiguration {Value = configuration.Default})
                });
            }
        }

        private void AddTextConfigurationToUsers(TextProjectConfiguration configuration , Project project ,
            string configurationName)
        {
            foreach (var projectProjectUser in project.ProjectUsers)
            {
                var projectCustomData =
                    project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName);
                projectCustomData.UserCustomData.Add(new UserCustomData
                {
                    UserId = projectProjectUser.UserId ,
                    SerializedConfiguration =
                        _jsonConvertService.Serialize(new TextUserConfiguration {Value = configuration.Default})
                });
            }
        }
    }
}