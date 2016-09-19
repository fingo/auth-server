using System;
using Fingo.Auth.DbAccess.Models.Statuses;

namespace Fingo.Auth.Domain.Models.AuditLog
{
    public class AuditLogModel
    {
        public int AuditLogId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public UserStatus UserStatus { get; set; }
        public string EventType { get; set; }
        public string EventMessage { get; set; }
        public DateTime CreationDate { get; set; }
    }
}