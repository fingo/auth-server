using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserChangedPassword : EventBase
    {
        public UserChangedPassword(int userId , string firstName , string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public override string ToString()
        {
            return $"User (id: {UserId}, name: {FirstName} {LastName}) changed his password.";
        }
    }
}