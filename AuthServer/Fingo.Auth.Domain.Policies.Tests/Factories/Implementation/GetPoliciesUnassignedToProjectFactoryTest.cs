using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Implementation;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.Factories.Implementation
{
    public class GetPoliciesUnassignedToProjectFactoryTest
    {
        [Fact]
        public void FactoryShouldReturnProperAction()
        {
            // arrange
            var repositoryMock = new Mock<IProjectRepository>();
            var factory = new GetPoliciesUnassignedToProjectFactory(repositoryMock.Object);

            // act
            var action = factory.Create();

            // assert
            Assert.NotNull(action);
            Assert.IsType<GetPoliciesUnassignedToProject>(action);
        }
    }
}
