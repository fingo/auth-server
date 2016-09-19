using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Policies.Factories.Implementation;
using Fingo.Auth.Domain.Policies.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.Factories.Implementation
{
    public class GetPoliciesFromProjectFactoryTest
    {
        [Fact]
        public void FactoryShouldReturnProperAction()
        {
            // arrange
            var repositoryMock = new Mock<IProjectRepository>();
            var convertMock = new Mock<IPolicyJsonConvertService>();
            var factory = new GetPoliciesFromProjectFactory(repositoryMock.Object, convertMock.Object);

            // act
            var action = factory.Create();

            // assert
            Assert.NotNull(action);
            Assert.IsType<GetPoliciesFromProject>(action);
        }
    }
}
