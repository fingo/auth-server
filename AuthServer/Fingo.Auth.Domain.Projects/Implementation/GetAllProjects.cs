using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class GetAllProjects : IGetAllProjects
    {
        private readonly IProjectRepository _repo;

        public GetAllProjects(IProjectRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<ProjectModel> Invoke()
        {
            var projects = _repo.GetAll().WithoutStatuses(ProjectStatus.Deleted);

            return projects.Select(project => new ProjectModel(project)).ToList();
        }
    }
}