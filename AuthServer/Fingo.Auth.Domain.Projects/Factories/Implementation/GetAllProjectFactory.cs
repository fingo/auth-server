using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class GetAllProjectFactory : IGetAllProjectFactory
    {
        private readonly IProjectRepository _repository;

        public GetAllProjectFactory(IProjectRepository repository)
        {
            _repository = repository;
        }

        public IGetAllProjects Create()
        {
            return new GetAllProjects(_repository);
        }
    }
}