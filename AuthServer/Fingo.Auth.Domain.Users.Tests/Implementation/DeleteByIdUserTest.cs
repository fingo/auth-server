using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class DeleteByIdUserTest
    {
        [Fact]
        public void Can_Delete_Project_From_User_But_No_Change_Status()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            User mockUser = new User
            {
                Id = 1,
                FirstName = "Pierwszy",
                LastName = "Pierwszy",
                Login = "jeden",
                Password = "jeden",
                ProjectUsers = new List<ProjectUser>()
                {
                    new ProjectUser() { ProjectId = 2, UserId = 1 },
                    new ProjectUser() { ProjectId = 1, UserId = 1 }
                },
                Status = UserStatus.Active
            };

            mockRepository.Setup(m => m.GetById(1)).Returns(mockUser);
           

            IDeleteByIdUser service = new Users.Implementation.DeleteByIdUser(mockRepository.Object,eventBusMock.Object);

            // Act

            service.Invoke(2, 1);
            User removedProjectFromUser = mockRepository.Object.GetById(1);

            var removedUser = mockRepository.Object.GetById(1);

            // Assert

            mockRepository.Verify(m => m.Edit(mockUser));
            Assert.True(removedProjectFromUser.ProjectUsers.Count() == 1);            
            Assert.True(removedUser.Status == UserStatus.Active);
           
        }

        [Fact]
        public void Can_Delete_User_With_Status_Active_with_One_Project()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IUserRepository> mockRepository = new Mock<IUserRepository>();
            User mockUser = new User
            {
                Id = 1,
                FirstName = "Pierwszy",
                LastName = "Pierwszy",
                Login = "jeden",
                Password = "jeden",
                ProjectUsers = new List<ProjectUser>()
                {
                    new ProjectUser() { ProjectId = 2, UserId = 1 }
                },
                Status = UserStatus.Active
            };

            mockRepository.Setup(m => m.GetById(1)).Returns(mockUser);

            IDeleteByIdUser service = new Users.Implementation.DeleteByIdUser(mockRepository.Object,eventBusMock.Object);

            // Act

            service.Invoke(2, 1);
            User removedProjectUser = mockRepository.Object.GetById(1);

            // Assert

            mockRepository.Verify(m => m.Edit(mockUser));
            Assert.True(!removedProjectUser.ProjectUsers.Any());
            Assert.True(removedProjectUser.Status == UserStatus.Active);
        }

        [Fact]
        public void Can_Delete_User_With_Status_Registered()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IUserRepository> mochRepository = new Mock<IUserRepository>();
            User mockUser = new User
            {
                Id = 1 ,
                FirstName = "Pierwszy" ,
                LastName = "Pierwszy" ,
                Login = "jeden" ,
                Password = "jeden" ,
                ProjectUsers = new List<ProjectUser>()
                {
                    new ProjectUser() { ProjectId = 2, UserId = 1 }
                } ,
                Status = UserStatus.Registered
            };

            mochRepository.Setup(m => m.GetById(1)).Returns(mockUser);

            IDeleteByIdUser service = new Users.Implementation.DeleteByIdUser(mochRepository.Object,eventBusMock.Object);

            // Act

            service.Invoke(2 , 1);
            User removedProjectUser = mochRepository.Object.GetById(1);

            // Assert

            mochRepository.Verify(m => m.Edit(mockUser));
            Assert.True(!removedProjectUser.ProjectUsers.Any());
            Assert.True(removedProjectUser.Status != UserStatus.Deleted);
            Assert.True(removedProjectUser.Status == UserStatus.Registered);
            Assert.True(true);
        }

        [Fact]
        public void Cannot_Delete_Wrong_User()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IUserRepository> mochRepository = new Mock<IUserRepository>();
            mochRepository.Setup(m => m.GetById(1)).Returns(() => null);
            IDeleteByIdUser service = new Users.Implementation.DeleteByIdUser(mochRepository.Object,eventBusMock.Object);

            // Act

            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => service.Invoke(2, 1));


            // Assert
            
            Assert.True(exc.Message.Contains("Cannot find user with id=1"));
        }

        [Fact]
        public void Cannot_Delete_User_From_Wrong_Project()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IUserRepository> mochRepository = new Mock<IUserRepository>();
            mochRepository.Setup(m => m.GetById(1)).Returns(new User
            {
                Id = 1,
                FirstName = "Pierwszy",
                LastName = "Pierwszy",
                Login = "jeden",
                Password = "jeden",
                ProjectUsers = new List<ProjectUser>()
            });
            
            IDeleteByIdUser service = new Users.Implementation.DeleteByIdUser(mochRepository.Object,eventBusMock.Object);

            // Act

            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => service.Invoke(5, 1));


            // Assert
            
            Assert.True(exc.Message.Contains("Cannot find project with id=5 connected with user id=1"));
        }
    }
}