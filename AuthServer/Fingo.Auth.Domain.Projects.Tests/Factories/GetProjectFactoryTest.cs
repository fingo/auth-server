using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Implementation;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Factories
{
    public class GetProjectFactoryTest
    {
        [Fact]
        public void GetProjectFactory_Returns_Instance_Of_GetProject_Given_By_IGetProject()
        {
            //Arrange

            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IGetProjectFactory target = new GetProjectFactory(projectMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetProject>(result);
            Assert.IsAssignableFrom<IGetProject>(result);
        }
    }
}