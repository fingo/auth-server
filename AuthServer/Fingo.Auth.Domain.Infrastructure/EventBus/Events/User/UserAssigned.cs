using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.User
{
    public class UserAssigned:EventBase
    {
        public UserAssigned(int projectId, string projectName, int userId)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            UserId = userId;
        }

        public int ProjectId { get; private set; }
        public string ProjectName { get; private set; }
        public int UserId { get; private set; }

        public override string ToString()
        {
            return $"User (id: {UserId}) was assigned to project (name: {ProjectName}, projectId: {ProjectId}).";
        }
    }
}