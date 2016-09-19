using System;
using Fingo.Auth.DbAccess.Models.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces
{
    public interface ISaveUserPolicyToProject
    {
         void Invoke(int projectId , int userId , Policy policy , DateTime userExpDate);
    }
}