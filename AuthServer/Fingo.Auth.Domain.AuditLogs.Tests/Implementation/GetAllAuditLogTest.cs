using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.AuditLogs.Implementation;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.AuditLogs.Tests.Implementation
{
    public class GetAllAuditLogTest
    {
        [Fact]
        public void Can_Get_All_AuditLogs()
        {
            //Arrange

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = 3 ,
                    FirstName = "firstName" ,
                    LastName = "lastName" ,
                    Status = UserStatus.Active
                }
            });
            var auditLogRepositoryMock = new Mock<IAuditLogRepository>();
            auditLogRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new AuditLog
                {
                    Id = 1 ,
                    UserId = 1 ,
                    EventMassage = "firstMessage" ,
                    EventType = "myType" ,
                    CreationDate = default(DateTime)
                } ,
                new AuditLog
                {
                    Id = 2 ,
                    UserId = 2 ,
                    EventMassage = "secondtMessage" ,
                    EventType = "myType" ,
                    CreationDate = default(DateTime)
                } ,
                new AuditLog
                {
                    Id = 3 ,
                    UserId = 3 ,
                    EventMassage = "thirdMessage" ,
                    EventType = "myType" ,
                    CreationDate = default(DateTime)
                }
            });

            //Act

            var target = new GetAllAuditLog(auditLogRepositoryMock.Object , userRepositoryMock.Object);
            var result = target.Invoke();

            //Assert

            Assert.True(result.Count() == 3);

            Assert.True(result.First().UserId == 0);
            Assert.True(result.First().UserName == "N/A");
            Assert.True(result.First().UserStatus == UserStatus.Deleted);

            Assert.True(result.Last().UserId == 3);
            Assert.True(result.Last().UserName == "firstName lastName");
            Assert.True(result.Last().UserStatus == UserStatus.Active);
        }
    }
}