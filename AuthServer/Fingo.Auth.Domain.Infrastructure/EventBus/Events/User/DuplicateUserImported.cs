using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class DuplicateUserImported : EventBase
    {
        public DuplicateUserImported(string firstName , string lastName , string login , int projectId)
        {
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            ProjectId = projectId;
        }

        public int ProjectId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Login { get; }

        public override string ToString()
        {
            return $"User (name: {FirstName} {LastName}) was not added to project (id: {ProjectId}) " +
                   $"because login: {Login} already exists in our database.";
        }
    }
}