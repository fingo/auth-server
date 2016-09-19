using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class ActivateByActivationTokenTest
    {
        [Fact]
        public void Can_Activate_User()
        {
            Mock<IUserRepository> userMock = new Mock<IUserRepository>();
            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            User user = new User
            {
                Id = 1,
                FirstName = "Pierwszy",
                LastName = "Pierwszy",
                Login = "jeden",
                Password = "jeden",
                ActivationToken = "859e3ecf-9558-4c22-9651-74fe600a94ab",
                Status = UserStatus.Registered

            };
            userMock.Setup(repo => repo.GetAll()).Returns(new List<User> {user});
            userMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(user);
            

            IActivateByActivationToken service = new ActivateByActivationToken(userMock.Object, eventBusMock.Object);

            service.Invoke(user.ActivationToken);

            var userToActivate = userMock.Object.GetById(7);

            Assert.True(userToActivate.FirstName == "Pierwszy");
            Assert.True(userToActivate.Status == UserStatus.Active);

        }
    }
}