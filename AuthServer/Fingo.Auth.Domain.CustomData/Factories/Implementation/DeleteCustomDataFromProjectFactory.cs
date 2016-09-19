using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Implementation
{
    public class DeleteCustomDataFromProjectFactory : IDeleteCustomDataFromProjectFactory
    {
        private readonly IProjectRepository projectRepository;
        private readonly IEventBus _eventBus;

        public DeleteCustomDataFromProjectFactory(IProjectRepository projectRepository, IEventBus eventBus)
        {
            _eventBus = eventBus;
            this.projectRepository = projectRepository;
        }

        public IDeleteCustomDataFromProject Create()
        {
            return new DeleteCustomDataFromProject(projectRepository, _eventBus);
        }
    }
}
