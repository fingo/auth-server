using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserRemovedFromProject:EventBase
    {
        public UserRemovedFromProject(string firstName , string lastName , string login, int projectId)
        {
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            ProjectId = projectId;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Login { get; private set; }
        public int ProjectId { get; private set; }

        public override string ToString()
        {
            return $"User (name: {FirstName} {LastName}, login: {Login}) was removed from project (id: {ProjectId}).";
        }
    }
}