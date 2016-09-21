using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Implementation;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Factories
{
    public class GetAllFromProjectUserFactoryTest
    {
        [Fact]
        public void
            GetAllFromProjectUserFactory_Returns_Instance_Of_GetAllFromProjectUser_Given_By_IGetAllFromProjectUser()
        {
            //Arrange

            var projectMock = new Mock<IProjectRepository>();

            //Act

            IGetAllFromProjectUserFactory target = new GetAllFromProjectUserFactory(projectMock.Object);
            var result = target.Create();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<GetAllFromProjectUser>(result);
            Assert.IsAssignableFrom<IGetAllFromProjectUser>(result);
        }
    }
}