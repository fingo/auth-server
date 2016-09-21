using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project
{
    public class ProjectAdded : EventBase
    {
        public ProjectAdded(int projectId , string projectName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
        }

        public int ProjectId { get; }
        public string ProjectName { get; }

        public override string ToString()
        {
            return $"New project (name: {ProjectName}, id: {ProjectId}) was added.";
        }
    }
}