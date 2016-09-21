using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface IGetPoliciesUnassignedToProject
    {
        List<Policy> Invoke(int projectId);
    }
}