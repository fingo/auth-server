using Fingo.Auth.Domain.AuditLogs.Interfaces;
using Fingo.Auth.Domain.Infrastructure.Interfaces;

namespace Fingo.Auth.Domain.AuditLogs.Factories.Interfaces
{
    public interface IGetAllAuditLogFactory : IActionFactory
    {
        IGetAllAuditLog Create();
    }
}