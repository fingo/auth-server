using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Factories.Implementation;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Factories
{
    public class GetAllProjectFactoryTest
    {
        [Fact]
        public void GetAllProjectFactory_Returns_Instance_Of_GetAllProjects_Given_By_IGetAllProjects()
        {
            //Arrange

            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IGetAllProjectFactory target = new GetAllProjectFactory(projectMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetAllProjects>(result);
            Assert.IsAssignableFrom<IGetAllProjects>(result);
        }
    }
}