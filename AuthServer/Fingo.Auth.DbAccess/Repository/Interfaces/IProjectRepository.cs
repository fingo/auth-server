using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces.GenericInterfaces;

namespace Fingo.Auth.DbAccess.Repository.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        IEnumerable<User> GetAllUsersFromProject(int id);
        Project GetByIdWithAll(int id);
        Project GetByGuid(Guid guid);
        Project GetByIdWithCustomDatas(int projectId);
        Project GetByIdWithPolicies(int id);
    }
}