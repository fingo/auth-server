using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class GetPolicyConfigurationOrDefaultFromProject : IGetPolicyConfigurationOrDefaultFromProject
    {
        private readonly IPolicyJsonConvertService jsonConvertService;
        private readonly IProjectRepository projectRepository;

        public GetPolicyConfigurationOrDefaultFromProject(IProjectRepository projectRepository ,
            IPolicyJsonConvertService jsonConvertService)
        {
            this.projectRepository = projectRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public PolicyConfiguration Invoke(int projectId , Policy policy)
        {
            var project = projectRepository.GetById(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var projectPolicy = project.ProjectPolicies.FirstOrDefault(pp => pp.Policy == policy);
            if (projectPolicy != null)
                try
                {
                    return jsonConvertService.Deserialize(projectPolicy.Policy ,
                        projectPolicy.SerializedProjectPolicySetting);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"There was a problem with deserializing policy configurations of project with id: {projectId}, " +
                        $"policy: {policy}, exception message: {e.Message}.");
                }

            switch (policy)
            {
                case Policy.AccountExpirationDate:
                    return new AccountExpirationDateConfiguration();
                case Policy.MinimumPasswordLength:
                    return new MinimumPasswordLengthConfiguration();
                case Policy.PasswordExpirationDate:
                    return new PasswordExpirationDateConfiguration();
                case Policy.RequiredPasswordCharacters:
                    return new RequiredPasswordCharactersConfiguration();
                case Policy.ExcludeCommonPasswords:
                    return new ExcludeCommonPasswordsConfiguration();
                default:
                    throw new Exception($"Wrong policy ({policy}) given to GetPolicyConfigurationOrDefaultFromProject.");
            }
        }
    }
}