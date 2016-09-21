using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserRemovedFromProject : EventBase
    {
        public UserRemovedFromProject(string firstName , string lastName , string login , int projectId)
        {
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            ProjectId = projectId;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Login { get; }
        public int ProjectId { get; }

        public override string ToString()
        {
            return $"User (name: {FirstName} {LastName}, login: {Login}) was removed from project (id: {ProjectId}).";
        }
    }
}