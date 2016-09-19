using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface IGetUserCustomDataConfigurationView
    {
        UserConfigurationView Invoke(int projectId , int userId , ConfigurationType configurationType ,
            string configurationName);
    }
}