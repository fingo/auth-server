using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Implementation;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Factories
{
    public class UpdateProjectFactoryTest
    {
        [Fact]
        public void UpdateProjectFactory_Returns_Instance_Of_UpdateProject_Given_By_IUpdateProject()
        {
            //Arrange

            var projectMock = new Mock<IProjectRepository>();

            //Act

            IUpdateProjectFactory target = new UpdateProjectFactory(projectMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<UpdateProject>(result);
            Assert.IsAssignableFrom<IUpdateProject>(result);
        }
    }
}