using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;

namespace Fingo.Auth.DbAccess.Repository.Interfaces
{
    public interface IAuditLogRepository
    {
        void Add(AuditLog entity);
        IEnumerable<AuditLog> GetAll();
    }
}