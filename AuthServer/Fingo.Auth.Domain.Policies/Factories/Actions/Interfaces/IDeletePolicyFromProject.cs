using Fingo.Auth.DbAccess.Models.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface IDeletePolicyFromProject
    {
        void Invoke(int projectId, Policy policy);
    }
}