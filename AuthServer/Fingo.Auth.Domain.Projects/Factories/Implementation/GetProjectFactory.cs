using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class GetProjectFactory : IGetProjectFactory
    {
        private readonly IProjectRepository _repository;
        public GetProjectFactory(IProjectRepository repository)
        {
            _repository = repository;
        }

        public IGetProject Create()
        {
            return new GetProject(_repository);
        }
    }
}