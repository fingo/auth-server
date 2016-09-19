using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.AuditLogs.Interfaces;
using Fingo.Auth.Domain.Models.AuditLog;

namespace Fingo.Auth.Domain.AuditLogs.Implementation
{
    public class GetAllAuditLog : IGetAllAuditLog
    {
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IUserRepository userRepository;
        public GetAllAuditLog(IAuditLogRepository auditLogRepository, IUserRepository userRepository)
        {
            this.auditLogRepository = auditLogRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<AuditLogModel> Invoke()
        {
            var logs = auditLogRepository.GetAll();

            Dictionary<int, User> users = userRepository.GetAll().ToDictionary(dic => dic.Id, dic => dic);

            Regex regex = new Regex(@"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", RegexOptions.Compiled);

            return (from auditLog in logs
                let user =
                users.ContainsKey(auditLog.UserId.GetValueOrDefault())
                    ? users[auditLog.UserId.GetValueOrDefault()]
                    : null
                select new AuditLogModel
                {
                    AuditLogId = auditLog.Id,
                    UserId = user?.Id ?? 0 ,
                    UserName = user != null ? $"{user.FirstName} {user.LastName}" : "N/A" ,
                    EventType =regex.Replace(auditLog.EventType ," $0") ,
                    EventMessage = auditLog.EventMassage ,
                    CreationDate = auditLog.CreationDate ,
                    UserStatus = user?.Status ?? UserStatus.Deleted
                });
        }
    }
}