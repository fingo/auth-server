using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserAdded : EventBase
    {
        public UserAdded(string firstName , string lastName , string login)
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
            return $"New user (name: {FirstName} {LastName}, login: {Login}) was created.";
        }
    }
}