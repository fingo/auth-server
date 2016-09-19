using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserChangedPassword : EventBase
    {
        public UserChangedPassword(int userId, string firstName, string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int UserId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public override string ToString()
        {
            return $"User (id: {UserId}, name: {FirstName} {LastName}) changed his password.";
        }
    }
}