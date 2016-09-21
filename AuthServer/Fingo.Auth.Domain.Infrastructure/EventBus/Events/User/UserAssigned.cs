using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserAssigned : EventBase
    {
        public UserAssigned(int projectId , string projectName , int userId)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            UserId = userId;
        }

        public int ProjectId { get; }
        public string ProjectName { get; }
        public int UserId { get; }

        public override string ToString()
        {
            return $"User (id: {UserId}) was assigned to project (name: {ProjectName}, projectId: {ProjectId}).";
        }
    }
}