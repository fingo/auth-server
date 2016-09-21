using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fingo.Auth.DbAccess.Context.Interfaces
{
    public interface IAuthServerContext
    {
        DbSet<AuditLog> AuditLog { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        EntityEntry Entry(object entity);
        int SaveChanges();

        void PerformMigration();
    }
}