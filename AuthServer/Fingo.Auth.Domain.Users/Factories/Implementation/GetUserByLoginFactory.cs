using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetUserByLoginFactory:IGetUserByLoginFactory
    {
        private readonly IUserRepository _repository;
        public GetUserByLoginFactory(IUserRepository repository)
        {
            _repository = repository;
        }
        public IGetUserByLogin Create()
        {
            return new GetUserByLogin(_repository);
        }
    }
}