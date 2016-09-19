using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class UpdateProject : IUpdateProject
    {
        private readonly IProjectRepository _repo;
        public UpdateProject(IProjectRepository repo)
        {
            _repo = repo;
        }

        public void Invoke(int id,string newName)
        {
            Project project = _repo.GetById(id).WithoutStatuses(ProjectStatus.Deleted);
            project.Name = newName;
            project.ProjectGuid= Guid.NewGuid();
            _repo.Edit(project);
        }
    }
}