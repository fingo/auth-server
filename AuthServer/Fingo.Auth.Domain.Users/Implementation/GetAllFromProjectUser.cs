using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetAllFromProjectUser : IGetAllFromProjectUser
    {
        private readonly IProjectRepository _repository;

        public GetAllFromProjectUser(IProjectRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserModel> Invoke(int projectId)
        {
            var users = _repository.GetAllUsersFromProject(projectId).WithoutStatuses(UserStatus.Deleted);
            return users.Select(user => new UserModel(user)).ToList();
        }
    }
}