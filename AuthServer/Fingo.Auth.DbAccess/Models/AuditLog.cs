using System;

namespace Fingo.Auth.DbAccess.Models
{
    public class AuditLog
    {
        public AuditLog()
        {
            CreationDate=DateTime.UtcNow;
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string EventType { get; set; }
        public string EventMassage { get; set; }
        public DateTime CreationDate { get; set; }
    }
}