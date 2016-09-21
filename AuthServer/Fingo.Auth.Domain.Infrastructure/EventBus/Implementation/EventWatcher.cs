using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Implementation
{
    public class EventWatcher : IEventWatcher
    {
        private readonly IAuditLogRepository auditLogRepository;

        public EventWatcher(IAuditLogRepository auditLogRepository)
        {
            this.auditLogRepository = auditLogRepository;
        }

        public void StoreEvent(string userId , string eventType , string eventMessage)
        {
            int parsedUserId;
            int.TryParse(userId , out parsedUserId);

            try
            {
                auditLogRepository.Add(new AuditLog
                {
                    EventType = eventType ,
                    UserId = parsedUserId ,
                    EventMassage = eventMessage
                });
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}