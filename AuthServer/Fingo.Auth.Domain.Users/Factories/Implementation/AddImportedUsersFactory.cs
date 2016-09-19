using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class AddImportedUsersFactory : IAddImportedUsersFactory
    {
        private readonly IUserRepository repository;
        private readonly IProjectRepository projectRepository;
        private readonly IEventBus eventBus;
        private readonly ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject;
        public AddImportedUsersFactory(IUserRepository repository, IProjectRepository projectRepository, IEventBus eventBus, ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
            this.eventBus = eventBus;
            this.setDefaultUserCustomDataBasedOnProject = setDefaultUserCustomDataBasedOnProject;
        }
        public IAddImportedUsers Create()
        {
            return new AddImportedUsers(repository, projectRepository, eventBus, setDefaultUserCustomDataBasedOnProject);
        }
    }
}