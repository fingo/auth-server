using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;

namespace Fingo.Auth.DbAccess.Repository.Implementation
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IAuthServerContext _db;

        public AuditLogRepository(IAuthServerContext context)
        {
            _db = context;
        }

        public virtual void Add(AuditLog entity)
        {
            _db.AuditLog.Add(entity);
            _db.SaveChanges();
        }

        public virtual IEnumerable<AuditLog> GetAll()
        {
            return _db.AuditLog.AsEnumerable();
        }
    }
}