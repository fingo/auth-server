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

        public int ProjectId { get; private set; }
        public string ProjectName { get; private set; }

        public override string ToString()
        {
            return $"Project (id: {ProjectId}, name: {ProjectName}) was removed.";
        }
    }
}