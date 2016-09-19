using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class GetProject:IGetProject
    {
        private readonly IProjectRepository _repo;
        public GetProject(IProjectRepository repo)
        {
            _repo = repo;
        }

        public ProjectDetailWithUsersModel Invoke(int id)
        {
            Project project = _repo.GetById(id).WithoutStatuses(ProjectStatus.Deleted);

            if (project == null)
            {
                throw new ArgumentNullException($"Cannot find project with id={id}.");
            }

            var users = _repo.GetAllUsersFromProject(id).WithoutStatuses(UserStatus.Deleted);

            ProjectDetailWithUsersModel projectDetailWithUsersModel=new ProjectDetailWithUsersModel(project,users);

            return projectDetailWithUsersModel;
        }
    }
}