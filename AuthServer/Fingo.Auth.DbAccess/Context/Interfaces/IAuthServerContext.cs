using Fingo.Auth.DbAccess.Models ;
using Fingo.Auth.DbAccess.Models.Base ;
using Microsoft.EntityFrameworkCore ;
using Microsoft.EntityFrameworkCore.ChangeTracking ;

namespace Fingo.Auth.DbAccess.Context.Interfaces
{
    public interface IAuthServerContext
    {
        DbSet<TEntity> Set <TEntity>( ) where TEntity : BaseEntity ;
        DbSet<AuditLog> AuditLog { get; set; }

        EntityEntry Entry( object entity );
        int SaveChanges();

        void PerformMigration();
    }
}