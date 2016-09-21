using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserMerged : EventBase
    {
        public UserMerged(string login , string firstName , string lastName)
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
            return $"New user (name: {FirstName} {LastName}, login: {Login}) was merged with deleted one.";
        }
    }
}