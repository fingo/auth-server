using System.Linq;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetUserByActivationToken : IGetUserByActivationToken
    {
        private readonly IUserRepository _repo;

        public GetUserByActivationToken(IUserRepository repo)
        {
            _repo = repo;
        }

        public UserModel Invoke(string activationToken)
        {
            var user = _repo.GetAll().FirstOrDefault(x => x.ActivationToken == activationToken);

            if (user == null)
                return null;

            var userModel = new UserModel(user) {Id = user.Id};
            return userModel;
        }
    }
}