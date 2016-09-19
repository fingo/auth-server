using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.AuditLogs.Factories.Implementation;
using Fingo.Auth.Domain.AuditLogs.Factories.Interfaces;
using Fingo.Auth.Domain.AuditLogs.Implementation;
using Fingo.Auth.Domain.AuditLogs.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.AuditLogs.Tests.Factories
{
    public class GetAllAuditLogFactoryTest
    {
        [Fact]
        public void GetAllAuditLogFactory_Should_Return_Instance_Of_GetAllAuditLog_Given_By_IGetAllAuditLog()
        {
            //Arrange

            Mock<IAuditLogRepository> auditLogRepository=new Mock<IAuditLogRepository>();
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();

            //Act

            IGetAllAuditLogFactory target = new GetAllAuditLogFactory(auditLogRepository.Object,userRepositoryMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetAllAuditLog>(result);
            Assert.IsAssignableFrom<IGetAllAuditLog>(result);
        }
    }
}