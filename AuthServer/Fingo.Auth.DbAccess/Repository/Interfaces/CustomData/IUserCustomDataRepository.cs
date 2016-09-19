using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Interfaces.GenericInterfaces;

namespace Fingo.Auth.DbAccess.Repository.Interfaces.CustomData
{
    public interface IUserCustomDataRepository : IGenericRepository<UserCustomData>
    {
        UserCustomData GetUserCustomData(int projectId , int userId , string configurationName);
    }
}