using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.Policies;
using Microsoft.EntityFrameworkCore;

namespace Fingo.Auth.DbAccess.Context
{
    public class AuthServerContext : DbContext, IAuthServerContext
    {
        public AuthServerContext()
        { }

        public AuthServerContext(DbContextOptions<AuthServerContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = AuthServer; Trusted_Connection = True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetPrimaryKeys(modelBuilder);

            SetProjectClientRelation(modelBuilder);

            SetProjectUserRelation(modelBuilder);

            SetPoliciesRelation(modelBuilder);

            SetProjectCustomDataRelation(modelBuilder);

            SetUserCustomDataRelation(modelBuilder);
        }

        private void SetUserCustomDataRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCustomData>()
                .HasOne(u => u.User)
                .WithMany(p => p.UserCustomData)
                .HasForeignKey(pu => pu.UserId);
        }

        private void SetProjectCustomDataRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectCustomData>()
                .HasOne(u => u.Project)
                .WithMany(p => p.ProjectCustomData)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<UserCustomData>()
                .HasOne(u => u.ProjectCustomData)
                .WithMany(p => p.UserCustomData)
                .HasForeignKey(pu => pu.ProjectCustomDataId);
        }

        private void SetPoliciesRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectPolicies>()
                .HasOne(u =>u.Project)
                .WithMany(p => p.ProjectPolicies)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<UserPolicies>()
                .HasOne(u => u.User)
                .WithMany(p => p.UserPolicies)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<UserPolicies>()
                .HasOne(u => u.ProjectPolicies)
                .WithMany(p => p.UserPolicies)
                .HasForeignKey(pu => pu.ProjectPoliciesId);
        }

        private void SetProjectUserRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>()
                .HasOne(u => u.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);
        }

        private void SetProjectClientRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Information)
                .WithOne(i => i.Project)
                .HasForeignKey<ClientInformation>(b => b.ProjectIdFk);
        }

        private void SetPrimaryKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientInformation>().HasKey(c => c.Id);
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<ProjectUser>().HasKey(pu => new { pu.ProjectId, pu.UserId });
            modelBuilder.Entity<AuditLog>().HasKey(i=>i.Id);
            modelBuilder.Entity<ProjectPolicies>().HasKey(pp => pp.Id);
            modelBuilder.Entity<UserCustomData>().HasKey(ucd => ucd.Id);
            modelBuilder.Entity<ProjectCustomData>().HasKey(pcd => pcd.Id);
            modelBuilder.Entity<UserCustomData>().HasKey(ucd => ucd.Id);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public DbSet<AuditLog> AuditLog { get; set; }

        public void PerformMigration()
        {
            Database.Migrate();
        }
    }
}