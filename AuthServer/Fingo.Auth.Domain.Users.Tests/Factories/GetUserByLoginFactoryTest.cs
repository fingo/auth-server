using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class GetUserByLoginFactoryTest
    {
        [Fact]
        public void GetUserByLoginFactory_Returns_Instance_Of_GetUserByLogin_Given_By_IGetUserByLogin()
        {
            //Arrange

            Mock<IUserRepository> userMock = new Mock<IUserRepository>();

            //Act

            IGetUserByLoginFactory target = new GetUserByLoginFactory(userMock.Object);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<GetUserByLogin>(result);
            Assert.IsAssignableFrom<IGetUserByLogin>(result);
        }
    }
}