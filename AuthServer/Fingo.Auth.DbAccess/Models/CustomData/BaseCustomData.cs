using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;

namespace Fingo.Auth.DbAccess.Models.CustomData
{
    public class BaseCustomData : BaseEntity
    {
        public string ConfigurationName { get; set; }

        public ConfigurationType ConfigurationType { get; set; }

        public string SerializedConfiguration { get; set; }
    }
}