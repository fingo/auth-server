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
    public class ChangePasswordToUserFactoryTest
    {
        [Fact]
        public void ChangePasswordToUserFactory_Returns_Instance_Of_AddUser_Given_By_IChangePasswordToUser()
        {
            var userMock = new Mock<IUserRepository>();
            var eventBusMock = new Mock<IEventBus>();

            IChangePasswordToUserFactory target = new ChangePasswordToUserFactory(userMock.Object , eventBusMock.Object);

            var result = target.Create();

            Assert.NotNull(result);
            Assert.IsType<ChangePasswordToUser>(result);
            Assert.IsAssignableFrom<IChangePasswordToUser>(result);
        }
    }
}