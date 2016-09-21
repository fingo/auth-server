using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserRemoved : EventBase
    {
        public UserRemoved(string firstName , string lastName , string login)
        {
            FirstName = firstName;
            LastName = lastName;
            Login = login;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Login { get; }

        public override string ToString()
        {
            return $"User (name: {FirstName} {LastName}, login: {Login}) was removed.";
        }
    }
}