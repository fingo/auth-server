using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Implementation;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Factories
{
    public class AddProjectFactoryTest
    {
        [Fact]
        public void AddProjectFactory_Returns_Instance_Of_AddProject_Given_By_IAddProject()
        {
            //Arrange

            var projectMock = new Mock<IProjectRepository>();
            var eventMock = new Mock<IEventBus>();

            //Act

            IAddProjectFactory target = new AddProjectFactory(projectMock.Object , eventMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<AddProject>(result);
            Assert.IsAssignableFrom<IAddProject>(result);
        }
    }
}