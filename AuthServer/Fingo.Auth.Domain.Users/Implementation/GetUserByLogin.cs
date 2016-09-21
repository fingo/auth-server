using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetUserByLogin : IGetUserByLogin
    {
        private readonly IUserRepository _repo;

        public GetUserByLogin(IUserRepository repo)
        {
            _repo = repo;
        }

        public UserModel Invoke(string login)
        {
            var user = _repo.GetAll().FirstOrDefault(x => x.Login == login).WithoutStatuses(UserStatus.Deleted);

            if (user == null)
                return null;

            var userModel = new UserModel(user) {Id = user.Id};
            return userModel;
        }
    }
}