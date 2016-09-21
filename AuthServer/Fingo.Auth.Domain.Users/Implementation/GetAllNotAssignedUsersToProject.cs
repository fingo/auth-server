using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetAllNotAssignedUsersToProject : IGetAllNotAssignedUsersToProject
    {
        private readonly IUserRepository repo;

        public GetAllNotAssignedUsersToProject(IUserRepository repo)
        {
            this.repo = repo;
        }

        public IEnumerable<BaseUserModel> Invoke(int projectId)
        {
            var users = repo.GetAll().WithoutStatuses(UserStatus.Deleted);

            var model = users.Where(user => user.ProjectUsers.All(m => m.ProjectId != projectId));

            var result = model.Select(user => new BaseUserModel(user));

            return result;
        }
    }
}