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
    public class GetUserCustomDataListFromProjectFactoryTest
    {
        [Fact]
        public void
            GetUserCustomDataListFromProjectFactory_Should_Return_Instance_Of_GetUserCustomDataListFromProject_Given_By_IGetUserCustomDataListFromProject
            ()
        {
            //Arrange

            var projectMock = new Mock<IProjectRepository>();
            var userMock = new Mock<IUserRepository>();
            var convertServiceMock = new Mock<ICustomDataJsonConvertService>();

            //Act

            IGetUserCustomDataListFromProjectFactory target =
                new GetUserCustomDataListFromProjectFactory(projectMock.Object ,
                    userMock.Object , convertServiceMock.Object);

            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetUserCustomDataListFromProject>(result);
            Assert.IsAssignableFrom<IGetUserCustomDataListFromProject>(result);
        }
    }
}