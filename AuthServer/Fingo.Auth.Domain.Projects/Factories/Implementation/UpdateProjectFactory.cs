using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class UpdateProjectFactory : IUpdateProjectFactory
    {
        private readonly IProjectRepository _repository;

        public UpdateProjectFactory(IProjectRepository repository)
        {
            _repository = repository;
        }

        public IUpdateProject Create()
        {
            return new UpdateProject(_repository);
        }
    }
}