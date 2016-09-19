using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class GetAllFromProjectUserFactory : IGetAllFromProjectUserFactory
    {
        private readonly IProjectRepository _repository;
        public GetAllFromProjectUserFactory(IProjectRepository repository)
        {
            _repository = repository;
        }
        public IGetAllFromProjectUser Create()
        {
            return new GetAllFromProjectUser(_repository);
        }
    }
}