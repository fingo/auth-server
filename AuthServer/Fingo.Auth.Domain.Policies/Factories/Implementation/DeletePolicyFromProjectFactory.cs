using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Implementation
{
    public class DeletePolicyFromProjectFactory : IDeletePolicyFromProjectFactory
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEventBus _eventBus;

        public DeletePolicyFromProjectFactory(IProjectRepository projectRepository, IEventBus eventBus)
        {
            _projectRepository = projectRepository;
            _eventBus = eventBus;
        }

        public IDeletePolicyFromProject Create()
        {
            return new DeletePolicyFromProject(_projectRepository, _eventBus);
        }
    }
}
