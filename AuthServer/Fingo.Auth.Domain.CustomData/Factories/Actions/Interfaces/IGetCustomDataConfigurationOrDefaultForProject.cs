using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface IGetCustomDataConfigurationOrDefaultForProject
    {
        CustomDataConfiguration Invoke(int projectId , string configurationName , ConfigurationType configurationType);
    }
}