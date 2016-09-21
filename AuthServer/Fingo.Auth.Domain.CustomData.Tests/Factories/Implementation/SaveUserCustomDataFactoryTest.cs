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
    public class SaveUserCustomDataFactoryTest
    {
        [Fact]
        public void SaveUserCustomDataFactory_Should_Return_Instance_Of_SaveUserCustomData_Given_By_ISaveUserCustomData()
        {
            //Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            var userMock = new Mock<IUserCustomDataRepository>();

            //Act

            ISaveUserCustomDataFactory target = new SaveUserCustomDataFactory(userMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<SaveUserCustomData>(result);
            Assert.IsAssignableFrom<ISaveUserCustomData>(result);
        }
    }
}