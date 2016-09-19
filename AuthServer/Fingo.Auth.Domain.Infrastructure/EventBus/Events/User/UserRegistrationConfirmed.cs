using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserRegistrationConfirmed:EventBase
    {
        public UserRegistrationConfirmed(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; private set; }

        public override string ToString()
        {
            return $"User account (login: {UserLogin}) was confirmed.";
        }
    }
}