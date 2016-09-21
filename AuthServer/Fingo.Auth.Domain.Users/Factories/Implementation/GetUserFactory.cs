using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetUserFactory : IGetUserFactory
    {
        private readonly IUserRepository _repository;

        public GetUserFactory(IUserRepository repository)
        {
            _repository = repository;
        }

        public IGetUser Create()
        {
            return new GetUser(_repository);
        }
    }
}