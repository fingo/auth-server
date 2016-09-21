using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.CustomData
{
    public class CustomDataRemoved : EventBase
    {
        public CustomDataRemoved(int projectId , string name)
        {
            ProjectId = projectId;
            Name = name;
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Custom data (name: {Name}) deleted from project (id: {ProjectId}).";
        }
    }
}