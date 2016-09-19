using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Policy;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class SavePolicyToProject : ISavePolicyToProject
    {
        private readonly IProjectRepository projectRepository;
        private readonly IEventBus eventBus;
        private readonly IPolicyJsonConvertService jsonConvertService;
        public SavePolicyToProject(IProjectRepository projectRepository, IEventBus eventBus,
            IPolicyJsonConvertService jsonConvertService)
        {
            this.jsonConvertService = jsonConvertService;
            this.eventBus = eventBus;
            this.projectRepository = projectRepository;
        }

        public void Invoke(int projectId, Policy policy, PolicyConfiguration policyConfiguration)
        {
            var project = projectRepository.GetById(projectId);

            if (project == null)
                throw new ArgumentNullException($"Cannot find project with id: {projectId}.");

            var projectPolicy = project.ProjectPolicies.FirstOrDefault(m => m.Policy == policy);
            if (projectPolicy != null)
            {
                project.ProjectPolicies.Remove(projectPolicy);
                project.ProjectPolicies.Add(new ProjectPolicies
                {
                    Policy = policy,
                    SerializedProjectPolicySetting = jsonConvertService.Serialize(policyConfiguration)
                });
            }
            else
            {
                project.ProjectPolicies.Add(new ProjectPolicies
                {
                    Policy = policy,
                    SerializedProjectPolicySetting = jsonConvertService.Serialize(policyConfiguration)
                });
            }
            projectRepository.Edit(project);
            eventBus.Publish(new PolicySaved(projectId, policy));
        }
    }
}