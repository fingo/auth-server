using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class AddUserFactory : IAddUserFactory
    {
        private readonly IUserRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEventBus _eventBus;
        private readonly ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject;
        public AddUserFactory(IUserRepository repository, IProjectRepository projectRepository, IEventBus eventBus, ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _eventBus = eventBus;
            this.setDefaultUserCustomDataBasedOnProject = setDefaultUserCustomDataBasedOnProject;
        }
        public IAddUser Create()
        {
            return new AddUser(_repository, _projectRepository,_eventBus, setDefaultUserCustomDataBasedOnProject);
        }
    }
}