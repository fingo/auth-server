using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Implementation
{
    public class GetUserCustomDataConfigurationViewFactoryTest
    {
        [Fact]
        public void GetUserCustomDataConfigurationViewFactory_Should_Return_Instance_Of_GetUserCustomDataConfigurationView_Given_By_IGetUserCustomDataConfigurationView()
        {
            //Arrange

            Mock<ICustomDataJsonConvertService> customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            Mock<IUserCustomDataRepository> userMock=new Mock<IUserCustomDataRepository>();
            Mock<IProjectRepository> projectMock = new Mock<IProjectRepository>();

            //Act

            IGetUserCustomDataConfigurationViewFactory target = new GetUserCustomDataConfigurationViewFactory(projectMock.Object,customDataJsonConvertServiceMock.Object,userMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetUserCustomDataConfigurationView>(result);
            Assert.IsAssignableFrom<IGetUserCustomDataConfigurationView>(result);
        }
    }
}