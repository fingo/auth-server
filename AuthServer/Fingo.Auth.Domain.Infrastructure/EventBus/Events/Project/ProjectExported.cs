using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project
{
    public class ProjectExported : EventBase
    {
        public ProjectExported(int projectId)
        {
            ProjectId = projectId;
        }

        public int ProjectId { get; set; }

        public override string ToString()
        {
            return $"Project (id: {ProjectId}) was exported to a CSV file.";
        }
    }
}