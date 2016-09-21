using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project
{
    public class ProjectRemoved : EventBase
    {
        public ProjectRemoved(int projectId , string projectName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
        }

        public int ProjectId { get; }
        public string ProjectName { get; }

        public override string ToString()
        {
            return $"Project (id: {ProjectId}, name: {ProjectName}) was removed.";
        }
    }
}