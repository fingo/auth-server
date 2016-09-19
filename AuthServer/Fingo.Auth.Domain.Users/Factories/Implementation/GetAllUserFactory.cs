using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetAllUserFactory : IGetAllUserFactory
    {
        private readonly IUserRepository userRepository;
        public GetAllUserFactory(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public IGetAllUser Create()
        {
            return new GetAllUser(userRepository);
        }
    }
}