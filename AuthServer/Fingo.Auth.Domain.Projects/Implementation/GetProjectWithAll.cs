using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class GetProjectWithAll : IGetProjectWithAll
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectWithAll(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public ProjectDetailWithAll Invoke(int projectId)
        {
            var project = _projectRepository.GetById(projectId).WithoutStatuses(ProjectStatus.Deleted);

            if (project == null)
                throw new ArgumentNullException($"Cannot find project with id={projectId}.");

            var users = _projectRepository.GetAllUsersFromProject(projectId)
                .WithStatuses(UserStatus.Active , UserStatus.Registered);
            var customData = _projectRepository.GetByIdWithCustomDatas(projectId).ProjectCustomData;
            var policies = project.ProjectPolicies.Select(pp => pp.Policy).Distinct();

            return new ProjectDetailWithAll(project , users , customData , policies);
        }
    }
}