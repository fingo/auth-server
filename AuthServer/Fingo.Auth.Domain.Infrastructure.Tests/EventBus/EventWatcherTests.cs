using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project;
using Fingo.Auth.Domain.Infrastructure.EventBus.Implementation;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Infrastructure.Tests.EventBus
{
    public class EventWatcherTests
    {
        [Fact]
        public void Can_Store_Event()
        {
            //Arrange
            AuditLog result=new AuditLog();
            Mock<IAuditLogRepository> repositoryMock=new Mock<IAuditLogRepository>();
            repositoryMock.Setup(m => m.Add(It.IsAny<AuditLog>())).Callback((AuditLog log) => result=log);

            //Act

            var target=new EventWatcher(repositoryMock.Object);
            target.StoreEvent("1",typeof(ProjectAdded).Name,"newMessage");

            //Assert

            repositoryMock.Verify(m=>m.Add(It.IsAny<AuditLog>()));
            Assert.True(result.EventMassage=="newMessage");
            Assert.True(result.UserId==1);
            Assert.True(result.EventType== "ProjectAdded");
        }
    }
}