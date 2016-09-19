using Fingo.Auth.Domain.CustomData.ConfigurationClasses;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces
{
    public interface ISaveUserCustomData
    {
        void Invoke(int projectId, int userId, string name,  UserConfiguration configuration);
    }
}
