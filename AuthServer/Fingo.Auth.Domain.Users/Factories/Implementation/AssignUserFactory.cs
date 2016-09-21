using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class AssignUserFactory : IAssignUserFactory
    {
        private readonly IEventBus eventBus;
        private readonly IProjectRepository projectRepository;
        private readonly ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject;
        private readonly IUserRepository userRepository;

        public AssignUserFactory(IEventBus eventBus , IProjectRepository projectRepository ,
            IUserRepository userRepository ,
            ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject)
        {
            this.eventBus = eventBus;
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.setDefaultUserCustomDataBasedOnProject = setDefaultUserCustomDataBasedOnProject;
        }

        public IAssignUser Create()
        {
            return new AssignUser(projectRepository , eventBus , userRepository , setDefaultUserCustomDataBasedOnProject);
        }
    }
}