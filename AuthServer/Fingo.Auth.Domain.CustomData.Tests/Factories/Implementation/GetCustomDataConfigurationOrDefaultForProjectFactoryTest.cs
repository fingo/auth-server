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
    public class GetCustomDataConfigurationOrDefaultForProjectFactoryTest
    {
        [Fact]
        public void
            GetCustomDataConfigurationOrDefaultForProjectFactory_Should_Return_Instance_Of_GetCustomDataConfigurationOrDefaultForProject_Given_By_IGetCustomDataConfigurationOrDefaultForProject
            ()
        {
            //Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            var projectMock = new Mock<IProjectRepository>();

            //Act

            IGetCustomDataConfigurationOrDefaultForProjectFactory target =
                new GetCustomDataConfigurationOrDefaultForProjectFactory(projectMock.Object ,
                    customDataJsonConvertServiceMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetCustomDataConfigurationOrDefaultForProject>(result);
            Assert.IsAssignableFrom<IGetCustomDataConfigurationOrDefaultForProject>(result);
        }
    }
}