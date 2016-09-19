using System.Collections.Generic;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class GetAllUser : IGetAllUser
    {
        private readonly IUserRepository userRepository;
        public GetAllUser(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<BaseUserModel> Invoke()
        {
            var users = userRepository.GetAll();

            var result = new List<BaseUserModel>();
            foreach (var user in users)
            {
                result.Add(new BaseUserModel(user));
            }
            return result;
        }
    }
}