using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class DeleteByIdUserFactoryTest
    {
        [Fact]
        public void DeleteByIdUserFactory_Returns_Instance_Of_DeleteByIdUser_Given_By_IDeleteByIdUser()
        {
            //Arrange

            var eventBusMock = new Mock<IEventBus>();
            var userMock = new Mock<IUserRepository>();

            //Act

            IDeleteByIdUserFactory target = new DeleteByIdUserFactory(userMock.Object , eventBusMock.Object);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<DeleteByIdUser>(result);
            Assert.IsAssignableFrom<IDeleteByIdUser>(result);
        }
    }
}