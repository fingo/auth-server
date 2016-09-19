using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class AddUserFactoryTests
    {
        [Fact]
        public void AddUserFactory_Returns_Instance_Of_AddUser_Given_By_IAddUser()
        {
            //Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IUserRepository> userMock = new Mock<IUserRepository>();
            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IAddUserFactory target = new AddUserFactory(userMock.Object , projectMock.Object,eventBusMock.Object,null);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AddUser>(result);
            Assert.IsAssignableFrom<IAddUser>(result);
        }
    }
}