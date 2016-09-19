using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface ISavePolicyToProject
    {
        void Invoke(int projectId, Policy policy, PolicyConfiguration policyConfiguration);
    }
}
