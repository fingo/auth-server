using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface IAddProjectCustomDataToProject
    {
        void Invoke(int projectId , string name , ConfigurationType type , ProjectConfiguration configuration);
    }
}