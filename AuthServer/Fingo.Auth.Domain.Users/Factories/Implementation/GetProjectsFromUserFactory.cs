using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetProjectsFromUserFactory : IGetProjectsFromUserFactory
    {
        private readonly IUserRepository userRepository;
        public GetProjectsFromUserFactory(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public IGetProjectsFromUser Create()
        {
            return new GetProjectsFromUser(userRepository);
        }
    }
}