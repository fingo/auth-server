using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.CustomData
{
    public class CustomDataAdded : EventBase
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public ConfigurationType Type { get; set; }

        public CustomDataAdded(int projectId, string name, ConfigurationType type)
        {
            ProjectId = projectId;
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"Custom data (name: {Name}, type: {Type}) added to project (id: {ProjectId}).";
        }
    }
}