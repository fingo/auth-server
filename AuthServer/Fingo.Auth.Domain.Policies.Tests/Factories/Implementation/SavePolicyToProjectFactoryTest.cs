using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Implementation;
using Fingo.Auth.Domain.Policies.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.Factories.Implementation
{
    public class SavePolicyToProjectFactoryTest
    {
        [Fact]
        public void FactoryShouldReturnProperAction()
        {
            // arrange
            var repositoryMock = new Mock<IProjectRepository>();
            var busMock = new Mock<IEventBus>();
            var convertMock = new Mock<IPolicyJsonConvertService>();
            var factory = new SavePolicyToProjectFactory(convertMock.Object, busMock.Object, repositoryMock.Object);

            // act
            var action = factory.Create();

            // assert
            Assert.NotNull(action);
            Assert.IsType<SavePolicyToProject>(action);
        }
    }
}
