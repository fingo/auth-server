using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;

namespace Fingo.Auth.Domain.Models.CustomData
{
    public class ProjectCustomDataModel
    {
        public ProjectCustomDataModel(ProjectCustomData customData)
        {
            ConfigurationName = customData.ConfigurationName;
            ConfigurationType = customData.ConfigurationType;
            SerializedConfiguration = customData.SerializedConfiguration;
            ProjectId = customData.ProjectId;
        }
        public string ConfigurationName { get; set; }

        public ConfigurationType ConfigurationType { get; set; }

        public string SerializedConfiguration { get; set; }
        public int ProjectId { get; set; }
    }
}