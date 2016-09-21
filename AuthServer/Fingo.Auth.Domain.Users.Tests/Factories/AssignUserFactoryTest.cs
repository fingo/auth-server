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
    public class AssignUserFactoryTest
    {
        [Fact]
        public void AssignUserFactory_Returns_Instance_Of_AssignUser_Given_By_IAssignUser()
        {
            //Arrange

            var eventBusMock = new Mock<IEventBus>();
            var projectMock = new Mock<IProjectRepository>();
            var userMock = new Mock<IUserRepository>();

            //Act

            IAssignUserFactory target = new AssignUserFactory(eventBusMock.Object , projectMock.Object , userMock.Object ,
                null);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssignUser>(result);
            Assert.IsAssignableFrom<IAssignUser>(result);
        }
    }
}