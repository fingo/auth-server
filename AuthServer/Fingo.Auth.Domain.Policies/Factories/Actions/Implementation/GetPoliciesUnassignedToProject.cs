using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class GetPoliciesUnassignedToProject : IGetPoliciesUnassignedToProject
    {
        private readonly IProjectRepository projectRepository;

        public GetPoliciesUnassignedToProject(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public List<Policy> Invoke(int projectId)
        {
            var project = projectRepository.GetById(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var projectPolicies = project.ProjectPolicies.Select(m => m.Policy);

            var allPolicies = Enum.GetValues(typeof(Policy)).Cast<Policy>().ToList();

            return allPolicies.Except(projectPolicies).ToList();
        }
    }
}