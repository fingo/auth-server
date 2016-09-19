using System;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetUser : IGetUser
    {
        private readonly IUserRepository _repo;
        public GetUser(IUserRepository repo)
        {
            _repo = repo;
        }
        public BaseUserModelWithProjects Invoke(int id)
        {
            var user = _repo.GetById(id).WithoutStatuses(UserStatus.Deleted);
            if(user==null)
                throw new ArgumentNullException($"Cannot find user with id={id}");

            var projects = _repo.GetAllProjectsFromUser(id).WithStatuses(ProjectStatus.Active);

            var projectDetailWithUsersModel = new BaseUserModelWithProjects(user, projects);

            return projectDetailWithUsersModel;
        }
    }
}