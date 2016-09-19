using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Implementation
{
    public class EditProjectCustomDataToProjectFactoryTest
    {
        [Fact]
        public void EditProjectCustomDataToProjectFactory_Should_Return_Instance_Of_EditProjectCustomDataToProject_Given_By_IEditProjectCustomDataToProject()
        {
            //Arrange

            Mock<ICustomDataJsonConvertService> customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IEditProjectCustomDataToProjectFactory target = new EditProjectCustomDataToProjectFactory(projectMock.Object,customDataJsonConvertServiceMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<EditProjectCustomDataToProject>(result);
            Assert.IsAssignableFrom<IEditProjectCustomDataToProject>(result);
        }
    }
}