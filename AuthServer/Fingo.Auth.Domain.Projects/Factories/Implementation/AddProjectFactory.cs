using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Implementation
{
    public class AddProjectFactory : IAddProjectFactory
    {
        private readonly IProjectRepository _repository;
        private IEventBus _eventBus;
        public AddProjectFactory(IProjectRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public IAddProject Create()
        {
            return new AddProject(_repository,_eventBus);
        }
    }
}