using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class GetUserPolicyConfigurationOrDefaultFromProject : IGetUserPolicyConfigurationOrDefaultFromProject
    {
        private readonly IPolicyJsonConvertService jsonConvertService;
        private readonly IUserRepository userRepository;

        public GetUserPolicyConfigurationOrDefaultFromProject(IUserRepository userRepository ,
            IPolicyJsonConvertService jsonConvertService)
        {
            this.userRepository = userRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public PolicyConfiguration Invoke(int userId , Policy policy , int projectId)
        {
            var user = userRepository.GetById(userId);
            if (user == null)
                throw new ArgumentNullException($"Could not find user with id: {projectId}.");

            var userPolicy =
                user.UserPolicies
                    .FirstOrDefault(
                        m => (m.ProjectPolicies.Policy == policy) && (m.ProjectPolicies.ProjectId == projectId));
            if (userPolicy != null)
                try
                {
                    return jsonConvertService.DeserializeUser(policy , userPolicy.SerializedUserPolicySetting);
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
                    return new UserAccountExpirationDateConfiguration
                    {
                        ExpirationDate = default(DateTime)
                    };
                default:
                    throw new Exception(
                        $"Wrong policy ({policy}) given to GetUserPolicyConfigurationOrDefaultFromProject.");
            }
        }
    }
}