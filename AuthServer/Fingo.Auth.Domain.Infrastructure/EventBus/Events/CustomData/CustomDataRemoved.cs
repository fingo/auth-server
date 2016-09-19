using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.CustomData
{
    public class CustomDataRemoved : EventBase
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }

        public CustomDataRemoved(int projectId, string name)
        {
            ProjectId = projectId;
            Name = name;
        }

        public override string ToString()
        {
            return $"Custom data (name: {Name}) deleted from project (id: {ProjectId}).";
        }
    }
}
