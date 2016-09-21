using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class GetCustomDataConfigurationOrDefaultForProject : IGetCustomDataConfigurationOrDefaultForProject
    {
        private readonly ICustomDataJsonConvertService jsonConvertService;
        private readonly IProjectRepository projectRepository;

        public GetCustomDataConfigurationOrDefaultForProject(IProjectRepository projectRepository ,
            ICustomDataJsonConvertService jsonConvertService)
        {
            this.projectRepository = projectRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public CustomDataConfiguration Invoke(int projectId , string configurationName ,
            ConfigurationType configurationType)
        {
            var project = projectRepository.GetByIdWithCustomDatas(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            if (string.IsNullOrEmpty(configurationName))
                switch (configurationType)
                {
                    case ConfigurationType.Boolean:
                        return new BooleanProjectConfiguration();
                    case ConfigurationType.Number:
                        return new NumberProjectConfiguration();
                    case ConfigurationType.Text:
                        return new TextProjectConfiguration();
                    default:
                        throw new Exception(
                            $"Wrong policy ({configurationType}) given to GetPolicyConfigurationOrDefaultFromProject.");
                }

            var projectCustomData =
                project.ProjectCustomData.FirstOrDefault(pp => pp.ConfigurationName == configurationName);
            if (projectCustomData != null)
                try
                {
                    return jsonConvertService.DeserializeProject(configurationType ,
                        projectCustomData.SerializedConfiguration);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"There was a problem with deserializing policy configurations of project with id: {projectId}, " +
                        $"policy: {configurationName}, exception message: {e.Message}.");
                }

            throw new Exception($"Something went wrong in GetCustomDataConfigurationOrDefaultForProject");
        }
    }
}