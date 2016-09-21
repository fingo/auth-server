using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class GetUserDataView : IGetUserDataView
    {
        private readonly IPolicyJsonConvertService jsonConvertService;
        private readonly IUserRepository userRepository;

        public GetUserDataView(IUserRepository userRepository ,
            IPolicyJsonConvertService jsonConvertService)
        {
            this.userRepository = userRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public UserDateView Invoke(int projectId , int userId)
        {
            var user = userRepository.GetById(userId);
            if (user == null)
                throw new ArgumentNullException($"Could not find user with id: {projectId}.");

            var userPolicy =
                user.UserPolicies.FirstOrDefault(
                    m =>
                        (m.ProjectPolicies.ProjectId == projectId) &&
                        (m.ProjectPolicies.Policy == Policy.AccountExpirationDate));

            if (userPolicy != null)
            {
                var result =
                    (UserAccountExpirationDateConfiguration)
                    jsonConvertService.DeserializeUser(Policy.AccountExpirationDate ,
                        userPolicy.SerializedUserPolicySetting);
                return new UserDateView
                {
                    UserId = userId ,
                    ProjectId = projectId ,
                    DateTime = result.ExpirationDate.ToString("dd-MM-yyyy")
                };
            }
            return new UserDateView
            {
                UserId = userId ,
                ProjectId = projectId
            };
        }
    }
}