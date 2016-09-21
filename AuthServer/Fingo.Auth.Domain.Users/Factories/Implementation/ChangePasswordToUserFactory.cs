using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Implementation
{
    public class ChangePasswordToUserFactory : IChangePasswordToUserFactory
    {
        private readonly IEventBus _eventBus;
        private readonly IUserRepository _userRepository;

        public ChangePasswordToUserFactory(IUserRepository userRepository , IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventBus = eventBus;
        }

        public IChangePasswordToUser Create()
        {
            return new ChangePasswordToUser(_userRepository , _eventBus);
        }
    }
}