using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.User;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class ActivateByActivationToken : IActivateByActivationToken
    {
        private readonly IEventBus _eventBus;
        private readonly IUserRepository _repository;

        public ActivateByActivationToken(IUserRepository repository , IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public void Invoke(string activationToken)
        {
            var user = _repository.GetAll().FirstOrDefault(u => u.ActivationToken == activationToken);
            if (user == null)
                throw new ArgumentNullException($"Cannot find user with activationToken={activationToken}");

            user.ModificationDate = DateTime.UtcNow;
            user.Status = UserStatus.Active;
            _repository.Edit(user);

            _eventBus.Publish(new UserRegistrationConfirmed(user.Login));
        }
    }
}