using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class AddUserTest
    {
        [Fact]
        public void Can_Add_User_With_Status_Registered()
        {
            // Arrange

            var eventBusMock = new Mock<IEventBus>();
            var userRepository = new Mock<IUserRepository>();
            var setDefaultUserCUstomDatea = new Mock<ISetDefaultUserCustomDataBasedOnProject>();

            userRepository.Setup(m => m.GetAll()).Returns(new List<User>());
            userRepository.Setup(m => m.Add(It.IsAny<User>()))
                .Callback((User u) => userRepository.Setup(m => m.GetAll()).Returns(new[] {u}));

            var projectRepository = new Mock<IProjectRepository>();
            projectRepository.Setup(m => m.GetByGuid(It.IsAny<Guid>())).Returns(new Project
            {
                Id = 1 ,
                Status = ProjectStatus.Active
            });

            IAddUser service = new AddUser(userRepository.Object , projectRepository.Object , eventBusMock.Object ,
                setDefaultUserCUstomDatea.Object);

            var userToAdd = new UserModel
            {
                FirstName = "drugi" ,
                LastName = "drugi" ,
                Login = "drugi"
            };

            // Act

            service.Invoke(userToAdd , new Guid());

            var users = userRepository.Object.GetAll();
            var addedUser = userRepository.Object.GetAll().Last();

            // Assert

            Assert.True(userToAdd.FirstName == addedUser.FirstName);
            Assert.True(userToAdd.LastName == addedUser.LastName);
            Assert.True(users.Count() == 1);
            Assert.True(addedUser.ProjectUsers.Count() == 1);
            Assert.True(addedUser.Status == UserStatus.Registered);
        }
    }
}