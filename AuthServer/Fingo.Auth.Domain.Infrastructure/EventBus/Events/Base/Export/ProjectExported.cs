namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base.Export
{
    public class ProjectExported : EventBase
    {
        public int ProjectId { get; set; }

        public ProjectExported(int projectId)
        {
            ProjectId = projectId;
        }

        public override string ToString()
        {
            return $"Project (id: {ProjectId}) was exported to a CSV file.";
        }
    }
}
