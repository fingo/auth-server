using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class GetProjectWithCustomData : IGetProjectWithCustomData
    {
        private readonly IProjectRepository projectRepository;

        public GetProjectWithCustomData(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public ProjectWithCustomDataModel Invoke(int projectId)
        {
            Project project = projectRepository.GetByIdWithCustomDatas(projectId).WithoutStatuses(ProjectStatus.Deleted);

            if (project == null)
            {
                throw new ArgumentNullException($"Cannot find project with id={projectId}.");
            }

            return new ProjectWithCustomDataModel(project);
        }
    }
}