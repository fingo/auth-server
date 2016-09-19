using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface IGetUserPolicyConfigurationOrDefaultFromProject
    {
        PolicyConfiguration Invoke(int userId , Policy policy , int projectId);
    }
}