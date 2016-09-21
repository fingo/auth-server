using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class ActivateByActivationTokenFactory : IActivateByActivationTokenFactory
    {
        private readonly IEventBus _eventBus;
        private readonly IUserRepository _repository;

        public ActivateByActivationTokenFactory(IUserRepository repository , IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public IActivateByActivationToken Create()
        {
            return new ActivateByActivationToken(_repository , _eventBus);
        }
    }
}