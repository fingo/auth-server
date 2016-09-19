using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Moq;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class ActivateByActivationTokenFactoryTest
    {
        [Fact]
        public void ActivateUserFactory_Returns_Instance_Of_ActivateUser_Given_By_IActivateUser()
        {
            Mock<IUserRepository> userMock = new Mock<IUserRepository>();
            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();

            IActivateByActivationTokenFactory target = new ActivateByActivationTokenFactory(userMock.Object, eventBusMock.Object);

            var result = target.Create();

            Assert.NotNull(result);
            Assert.IsType<ActivateByActivationToken>(result);
            Assert.IsAssignableFrom<IActivateByActivationToken>(result);
        }
    }
}
