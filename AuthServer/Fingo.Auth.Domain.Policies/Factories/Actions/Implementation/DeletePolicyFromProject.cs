using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Policy;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class DeletePolicyFromProject : IDeletePolicyFromProject
    {
        private readonly IProjectRepository projectRepository;
        private readonly IEventBus eventBus;
        public DeletePolicyFromProject(IProjectRepository projectRepository, IEventBus eventBus)
        {
            this.eventBus = eventBus;
            this.projectRepository = projectRepository;
        }

        public void Invoke(int projectId, Policy policy)
        {
            var project = projectRepository.GetById(projectId);

            if (project == null)
                throw new ArgumentNullException($"Cannot find project with id: {projectId}.");

            if (project.ProjectPolicies.All(m => m.Policy != policy))
                throw new ArgumentNullException($"This project doesn't have such policy.");

            var projectPolicy = project.ProjectPolicies.FirstOrDefault(m => m.Policy == policy);

            project.ProjectPolicies.Remove(projectPolicy);

            projectRepository.Edit(project);
            eventBus.Publish(new PolicyRemoved(projectId, policy));
        }
    }
}