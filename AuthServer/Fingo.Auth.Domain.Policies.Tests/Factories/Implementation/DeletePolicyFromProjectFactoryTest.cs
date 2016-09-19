using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Implementation;
using Xunit;
using Moq;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;

namespace Fingo.Auth.Domain.Policies.Tests.Factories.Implementation
{
    public class DeletePolicyFromProjectFactoryTest
    {
        [Fact]
        public void FactoryShouldReturnProperAction()
        {
            // arrange
            var repositoryMock = new Mock<IProjectRepository>();
            var busMock = new Mock<IEventBus>();
            var factory = new DeletePolicyFromProjectFactory(repositoryMock.Object, busMock.Object);

            // act
            var action = factory.Create();

            // assert
            Assert.NotNull(action);
            Assert.IsType<DeletePolicyFromProject>(action);
        }
    }
}
