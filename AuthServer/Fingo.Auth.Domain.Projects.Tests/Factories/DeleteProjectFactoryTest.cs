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
    public class DeleteProjectFactoryTest
    {
        [Fact]
        public void DeleteProjectFactory_Returns_Instance_Of_DeleteProject_Given_By_IDeleteProject()
        {
            //Arrange

            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();
            Mock<IEventBus> eventBusMock=new Mock<IEventBus>();

            //Act

            IDeleteProjectFactory target = new DeleteProjectFactory(projectMock.Object,eventBusMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<DeleteProject>(result);
            Assert.IsAssignableFrom<IDeleteProject>(result);
        }
    }
}