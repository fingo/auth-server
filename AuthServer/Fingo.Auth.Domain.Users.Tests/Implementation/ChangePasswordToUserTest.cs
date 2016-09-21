using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class ChangePasswordToUserTest
    {
        [Fact]
        public void Change_Password()
        {
            var userMock = new Mock<IUserRepository>();
            var eventBusMock = new Mock<IEventBus>();

            var user1 = new User
            {
                Id = 1 ,
                Login = "user1" ,
                Password = "aaa" ,
                Status = UserStatus.Active
            };

            userMock.Setup(m => m.GetAll()).Returns(new[]
            {
                user1
            });

            var changingPasswordUser = new ChangingPasswordUser
            {
                Email = "user1" ,
                Password = "aaa" ,
                NewPassword = "zzzz"
            };

            IChangePasswordToUser target = new ChangePasswordToUser(userMock.Object , eventBusMock.Object);

            target.Invoke(changingPasswordUser);

            Assert.True(user1.Password == "zzzz");
            userMock.Verify(x => x.Edit(user1));
        }


        [Fact]
        public void change_Password_to_Non_Existing_User()
        {
            var userMock = new Mock<IUserRepository>();
            var eventBusMock = new Mock<IEventBus>();

            userMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User {Id = 1 , Login = "user1" , Status = UserStatus.Active}
            });

            IChangePasswordToUser target = new ChangePasswordToUser(userMock.Object , eventBusMock.Object);

            var changingPasswordUser = new ChangingPasswordUser
            {
                Email = "user2"
            };


            Exception ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(changingPasswordUser));
            Assert.True(
                ex.Message.Contains(
                    "Cannot change password for user with email: user2, because this user does not exist."));
        }

        [Fact]
        public void Change_Password_With_Uncorrect_Data()
        {
            var userMock = new Mock<IUserRepository>();
            var eventBusMock = new Mock<IEventBus>();

            userMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = 1 ,
                    Login = "user1" ,
                    Password = "aaa" ,
                    Status = UserStatus.Active
                }
            });

            var changingPasswordUser = new ChangingPasswordUser
            {
                Email = "user1" ,
                Password = "zz"
            };

            IChangePasswordToUser target = new ChangePasswordToUser(userMock.Object , eventBusMock.Object);

            var ex = Assert.Throws<Exception>(() => target.Invoke(changingPasswordUser));
            Assert.True(
                ex.Message.Contains("Cannot change password for user with email: user1, because password was wrong."));
        }
    }
}