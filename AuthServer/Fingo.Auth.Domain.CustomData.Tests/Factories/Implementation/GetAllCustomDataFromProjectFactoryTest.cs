using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Implementation;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Implementation
{
    public class GetAllCustomDataFromProjectFactoryTest
    {
        [Fact]
        public void
            GetAllCustomDataFromProjectFactory_Should_Return_Instance_Of_GetAllCustomDataFromProject_Given_By_IGetAllCustomDataFromProject
            ()
        {
            //Arrange

            var projectMock = new Mock<IProjectRepository>();

            //Act

            IGetAllCustomDataFromProjectFactory target = new GetAllCustomDataFromProjectFactory(projectMock.Object);
            var result = target.Create();

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetAllCustomDataFromProject>(result);
            Assert.IsAssignableFrom<IGetAllCustomDataFromProject>(result);
        }
    }
}