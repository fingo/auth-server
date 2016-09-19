using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Implementation.GenericImplementation;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;

namespace Fingo.Auth.DbAccess.Repository.Implementation.CustomData
{
    public class ProjectCustomDataRepository : GenericRepository<ProjectCustomData>,IProjectCustomDataRepository
    {
        private IAuthServerContext _db;
        public ProjectCustomDataRepository(IAuthServerContext context) : base(context)
        {
            _db = context;
        }
    }
}