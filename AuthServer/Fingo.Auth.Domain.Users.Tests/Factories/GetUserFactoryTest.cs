using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class GetUserFactoryTest
    {
        [Fact]
        public void GetUserFactory_Returns_Instance_Of_GetUser_Given_By_IGetUser()
        {
            //Arrange

            var userMock = new Mock<IUserRepository>();

            //Act

            IGetUserFactory target = new GetUserFactory(userMock.Object);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<GetUser>(result);
            Assert.IsAssignableFrom<IGetUser>(result);
        }
    }
}