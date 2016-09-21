using Fingo.Auth.Domain.Policies.ConfigurationClasses;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface IGetUserDataView
    {
        UserDateView Invoke(int projectId , int userId);
    }
}