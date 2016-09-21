using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class EditProjectCustomDataToProject : IEditProjectCustomDataToProject
    {
        private readonly ICustomDataJsonConvertService _jsonConvertService;
        private readonly IProjectRepository _projectRepository;

        public EditProjectCustomDataToProject(IProjectRepository projectRepository ,
            ICustomDataJsonConvertService jsonConvertService)
        {
            _jsonConvertService = jsonConvertService;
            _projectRepository = projectRepository;
        }

        public void Invoke(int projectId , string name , ConfigurationType type , ProjectConfiguration configuration ,
            string oldName)
        {
            var project = _projectRepository.GetByIdWithCustomDatas(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var customData = project.ProjectCustomData.FirstOrDefault(data => data.ConfigurationName == oldName);
            if (customData == null)
                throw new Exception($"You cannot edit non-existing custom data (name: {oldName}).");

            customData.ConfigurationName = name;
            customData.ConfigurationType = type;
            customData.SerializedConfiguration = _jsonConvertService.Serialize(configuration);

            EditConfigurationForUsers(name , type , configuration , project);

            _projectRepository.Edit(project);
        }

        private void EditConfigurationForUsers(string name , ConfigurationType type , ProjectConfiguration configuration ,
            Project project)
        {
            switch (type)
            {
                case ConfigurationType.Boolean:
                {
                    break;
                }
                case ConfigurationType.Number:
                {
                    EditNumberConfigurationToUsers((NumberProjectConfiguration) configuration , project , name);
                    break;
                }
                case ConfigurationType.Text:
                {
                    EditTextConfigurationToUsers((TextProjectConfiguration) configuration , project , name);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type) , type , null);
            }
        }

        //private void EditBooleanConfigurationToUsers(Project project , string configurationName , string oldName)
        //{
        //    if (configurationName == oldName)
        //        return;

        //    foreach (var projectProjectUser in project.ProjectUsers)
        //    {
        //        foreach (var projectCustomData in project.ProjectCustomData.Where(m=> m.ConfigurationName == oldName))
        //        {
        //            try
        //            {
        //                var userCustomData =
        //                    projectCustomData.UserCustomData.FirstOrDefault(m => m.UserId == projectProjectUser.UserId);

        //                projectCustomData.UserCustomData.Remove(userCustomData);
        //                userCustomData.ModificationDate = DateTime.UtcNow;
        //                projectCustomData.UserCustomData.Add(userCustomData);
        //            }
        //            catch (Exception)
        //            {
        //                //ignore
        //            }
        //        }
        //    }
        //}

        private void EditNumberConfigurationToUsers(NumberProjectConfiguration configuration , Project project ,
            string configurationName)
        {
            var projectCustomData =
                project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName);
            foreach (var userCustomData in projectCustomData.UserCustomData)
                try
                {
                    var userConfiguration =
                        (NumberUserConfiguration)
                        _jsonConvertService.DeserializeUser(ConfigurationType.Number ,
                            userCustomData.SerializedConfiguration);

                    var userValueUpdateIsNeeded = (userConfiguration.Value < configuration.LowerBound) ||
                                                  (userConfiguration.Value > configuration.UpperBound);

                    if (!userValueUpdateIsNeeded)
                        continue;

                    userConfiguration.Value = configuration.Default;
                    userCustomData.SerializedConfiguration = _jsonConvertService.Serialize(userConfiguration);
                    userCustomData.ModificationDate = DateTime.UtcNow;
                }
                catch (Exception)
                {
                    //ignore
                }
        }

        private void EditTextConfigurationToUsers(TextProjectConfiguration configuration , Project project ,
            string configurationName)
        {
            var projectCustomData =
                project.ProjectCustomData.FirstOrDefault(m => m.ConfigurationName == configurationName);
            foreach (var userCustomData in projectCustomData.UserCustomData)
                try
                {
                    var userConfiguration =
                        (TextUserConfiguration)
                        _jsonConvertService.DeserializeUser(ConfigurationType.Text ,
                            userCustomData.SerializedConfiguration);

                    var userValueUpdateIsNeeded = !(configuration.PossibleValues.Contains(userConfiguration.Value) ||
                                                    (configuration.Default == userConfiguration.Value));

                    if (!userValueUpdateIsNeeded)
                        continue;

                    userConfiguration.Value = configuration.Default;
                    userCustomData.SerializedConfiguration = _jsonConvertService.Serialize(userConfiguration);
                    userCustomData.ModificationDate = DateTime.UtcNow;
                }
                catch (Exception)
                {
                    //ignore
                }
        }
    }
}