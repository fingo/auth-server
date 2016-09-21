using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetUserByActivationTokenFactory : IGetUserByActivationTokenFactory
    {
        private readonly IUserRepository _repository;

        public GetUserByActivationTokenFactory(IUserRepository repository)
        {
            _repository = repository;
        }

        public IGetUserByActivationToken Create()
        {
            return new GetUserByActivationToken(_repository);
        }
    }
}