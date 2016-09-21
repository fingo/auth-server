using System.Linq;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Implementation.GenericImplementation;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Microsoft.EntityFrameworkCore;

namespace Fingo.Auth.DbAccess.Repository.Implementation.CustomData
{
    public class UserCustomDataRepository : GenericRepository<UserCustomData> , IUserCustomDataRepository
    {
        private readonly IAuthServerContext _db;

        public UserCustomDataRepository(IAuthServerContext context) : base(context)
        {
            _db = context;
        }

        public UserCustomData GetUserCustomData(int projectId , int userId , string configurationName)
        {
            var userCustomDatas =
                _db.Set<UserCustomData>().Where(m => m.UserId == userId).Include(m => m.ProjectCustomData);
            return
                userCustomDatas
                    .FirstOrDefault(
                        m =>
                            (m.ProjectCustomData.ProjectId == projectId) && (m.UserId == userId) &&
                            (m.ProjectCustomData.ConfigurationName == configurationName));
        }
    }
}