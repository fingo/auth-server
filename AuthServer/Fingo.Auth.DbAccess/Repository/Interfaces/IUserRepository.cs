using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models ;
using Fingo.Auth.DbAccess.Repository.Interfaces.GenericInterfaces ;

namespace Fingo.Auth.DbAccess.Repository.Interfaces
{
    public interface IUserRepository:IGenericRepository<User>
    {
        void UpdateUserPassword(User user, string password);
        IEnumerable<Project> GetAllProjectsFromUser(int id);
        User GetByIdWithCustomDatas(int id);
    }
}