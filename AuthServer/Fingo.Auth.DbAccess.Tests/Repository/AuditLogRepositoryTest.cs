using System.Linq;
using Fingo.Auth.DbAccess.Context;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Implementation;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fingo.Auth.DbAccess.Tests.Repository
{
    public class AuditLogRepositoryTest
    {
        private AuthServerContext context;

        public AuditLogRepositoryTest()
        {
            var serviceProvider = new ServiceCollection()
               .AddEntityFrameworkInMemoryDatabase()
               .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<AuthServerContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            context = new AuthServerContext(builder.Options);
        }
        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void Can_Add_AuditLog()
        {
            //Arrange

            IAuditLogRepository auditLogRepository=new AuditLogRepository(context);
            AuditLog log = new AuditLog() {UserId = 1,EventMassage = "test",EventType = "test"};

            //Act

            auditLogRepository.Add(log);
            var objectInDb = context.AuditLog.First();

            //Assert
            
            Assert.True(context.AuditLog.Count()==1);
            Assert.True(objectInDb.Id==1);
            Assert.True(objectInDb.EventMassage=="test");
            Assert.True(objectInDb.EventType=="test");
        }
    }
}