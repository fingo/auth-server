using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.AuditLogs.Factories.Interfaces;
using Fingo.Auth.Domain.AuditLogs.Implementation;
using Fingo.Auth.Domain.AuditLogs.Interfaces;

namespace Fingo.Auth.Domain.AuditLogs.Factories.Implementation
{
    public class GetAllAuditLogFactory : IGetAllAuditLogFactory
    {
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IUserRepository userRepository;

        public GetAllAuditLogFactory(IAuditLogRepository auditLogRepository , IUserRepository userRepository)
        {
            this.auditLogRepository = auditLogRepository;
            this.userRepository = userRepository;
        }

        public IGetAllAuditLog Create()
        {
            return new GetAllAuditLog(auditLogRepository , userRepository);
        }
    }
}