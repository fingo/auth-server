using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class DeleteProjectFactory : IDeleteProjectFactory
    {
        private readonly IEventBus _eventBus;
        private readonly IProjectRepository _repository;

        public DeleteProjectFactory(IProjectRepository repository , IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public IDeleteProject Create()
        {
            return new DeleteProject(_repository , _eventBus);
        }
    }
}