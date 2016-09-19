using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Implementation
{
    public class AddProjectCustomDataToProjectFactoryTest
    {
        [Fact]
        public void AddProjectCustomDataToProjectFactory_Should_Return_Instance_Of_AddProjectCustomDataToProject_Given_By_IAddProjectCustomDataToProject()
        {
            //Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<ICustomDataJsonConvertService> customDataJsonConverterMock = new Mock<ICustomDataJsonConvertService>();
            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IAddProjectCustomDataToProjectFactory target = new AddProjectCustomDataToProjectFactory(projectMock.Object, customDataJsonConverterMock.Object, eventBusMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<AddProjectCustomDataToProject>(result);
            Assert.IsAssignableFrom<IAddProjectCustomDataToProject>(result);
        }
    }
}