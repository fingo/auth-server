using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class GetUserTest
    {
        [Fact]
        public void Get_NonExisting_User()
        {
            var userMock = new Mock<IUserRepository>();

            userMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            IGetUser target = new GetUser(userMock.Object);

            Exception ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(1));

            Assert.True(ex.Message.Contains("Cannot find user with id=1"));
        }

        [Fact]
        public void Get_User()
        {
            var userMock = new Mock<IUserRepository>();

            userMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new User
            {
                Id = 1 ,
                Login = "user1"
            });
            IGetUser target = new GetUser(userMock.Object);

            var result = target.Invoke(1);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.True(result.Login == "user1");
        }
    }
}