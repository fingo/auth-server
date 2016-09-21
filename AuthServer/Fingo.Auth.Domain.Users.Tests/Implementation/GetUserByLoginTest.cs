using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class GetUserByLoginTest
    {
        [Fact]
        public void Can_Get_User_By_Correct_Login()
        {
            //Arrange

            var userMock = new Mock<IUserRepository>();
            userMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User {Id = 1 , Login = "user1" , Status = UserStatus.Active}
            });
            IGetUserByLogin target = new GetUserByLogin(userMock.Object);

            //Act

            var result = target.Invoke("user1");

            //Assert

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public void Finds_User_With_Status_Registered()
        {
            //Arrange
            var mockUser = new Mock<IUserRepository>();

            mockUser.Setup(m => m.GetAll()).Returns(new[]
            {
                new User {Id = 1 , Login = "user1" , Status = UserStatus.Registered}
            });

            IGetUserByLogin target = new GetUserByLogin(mockUser.Object);

            //Act

            var result = target.Invoke("user1");

            //Assert

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public void Receive_null_When_UserStatus_Equals_Deleted()
        {
            //Arrange
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = 1 ,
                    Login = "user1" ,
                    Status = UserStatus.Deleted
                }
            });

            IGetUserByLogin target = new GetUserByLogin(mockRepository.Object);

            //Act
            var result = target.Invoke("user1");

            //Assert

            Assert.Null(result);
        }

        [Fact]
        public void Recive_Null_When_User_With_Given_Loggin_Not_Exist()
        {
            //Arrange

            var userMock = new Mock<IUserRepository>();
            userMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User {Id = 1 , Login = "user2" , Status = UserStatus.Active}
            });
            IGetUserByLogin target = new GetUserByLogin(userMock.Object);

            //Act

            var result = target.Invoke("user1");

            //Assert

            Assert.Null(result);
        }
    }
}