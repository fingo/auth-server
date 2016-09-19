using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class DeleteByIdUserFactory : IDeleteByIdUserFactory
    {
        private readonly IUserRepository _repository;
        private readonly IEventBus _eventBus;

        public DeleteByIdUserFactory(IUserRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }
        public IDeleteByIdUser Create()
        {
            return new DeleteByIdUser(_repository,_eventBus);
        }
    }
}