using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class GetAllNotAssignedUsersToProjectFactoryTest
    {
        [Fact]
        public void GetAllNotAssignedUsersToProjectFactory_Returns_Instance_Of_GetAllNotAssignedUsersToProject_Given_By_IGetAllNotAssignedUsersToProject()
        {
            //Arrange

            Mock<IUserRepository> userMock = new Mock<IUserRepository>();

            //Act

            IGetAllNotAssignedUsersToProjectFactory target = new GetAllNotAssignedUsersToProjectFactory(userMock.Object);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<GetAllNotAssignedUsersToProject>(result);
            Assert.IsAssignableFrom<IGetAllNotAssignedUsersToProject>(result);
        }
    }
}