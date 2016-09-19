using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class SaveUserPolicyToProject : ISaveUserPolicyToProject
    {
        private readonly IProjectRepository projectRepository;
        private readonly IPolicyJsonConvertService jsonConvertService;
        public SaveUserPolicyToProject(IProjectRepository projectRepository,
            IPolicyJsonConvertService jsonConvertService)
        {
            this.projectRepository = projectRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public void Invoke(int projectId, int userId, Policy policy, DateTime userExpDate)
        {
            var project = projectRepository.GetByIdWithPolicies(projectId);

            if (project == null)
                throw new ArgumentNullException($"Cannot find user with id:{projectId}");

            UserPolicies userPolicy = project.ProjectPolicies.FirstOrDefault(m => m.Policy == policy)
                .UserPolicies.FirstOrDefault(m => m.UserId == userId);

            if (userPolicy != null)
            {
                project.ProjectPolicies.FirstOrDefault(m => m.Policy == policy).UserPolicies.Remove(userPolicy);
                if (userExpDate == default(DateTime))
                {
                    projectRepository.Edit(project);
                    return;
                }
                UserAccountExpirationDateConfiguration result =(UserAccountExpirationDateConfiguration)jsonConvertService.DeserializeUser(policy, userPolicy.SerializedUserPolicySetting);
                result.ExpirationDate = userExpDate;
                userPolicy.SerializedUserPolicySetting = jsonConvertService.Serialize(result);
                project.ProjectPolicies.FirstOrDefault(m => m.Policy == policy).UserPolicies.Add(userPolicy);
            }
            else
            {
                if (userExpDate == default(DateTime))
                {
                    return;
                }
                userPolicy = new UserPolicies()
                {
                    UserId = userId,
                    SerializedUserPolicySetting = jsonConvertService.Serialize(new UserAccountExpirationDateConfiguration() {ExpirationDate = userExpDate})
                };
                project.ProjectPolicies.FirstOrDefault(m => m.Policy == policy).UserPolicies.Add(userPolicy);
            }
            projectRepository.Edit(project);
        }
    }
}