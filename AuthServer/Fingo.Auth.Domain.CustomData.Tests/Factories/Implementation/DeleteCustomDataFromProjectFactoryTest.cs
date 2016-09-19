using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Implementation
{
    public class DeleteCustomDataFromProjectFactoryTest
    {
        [Fact]
        public void DeleteCustomDataFromProjectFactory_Should_Return_Instance_Of_DeleteCustomDataFromProject_Given_By_IDeleteCustomDataFromProject()
        {
            //Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IDeleteCustomDataFromProjectFactory target = new DeleteCustomDataFromProjectFactory(projectMock.Object, eventBusMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<DeleteCustomDataFromProject>(result);
            Assert.IsAssignableFrom<IDeleteCustomDataFromProject>(result);
        }
    }
}