using System.Collections.Generic;
using Fingo.Auth.Domain.Models.AuditLog;

namespace Fingo.Auth.Domain.AuditLogs.Interfaces
{
    public interface IGetAllAuditLog
    {
        IEnumerable<AuditLogModel> Invoke();
    }
}