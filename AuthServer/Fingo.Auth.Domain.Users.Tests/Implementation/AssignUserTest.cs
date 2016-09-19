using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class AssignUserTest
    {
        private readonly Mock<IEventBus> eventBus;
        public AssignUserTest()
        {
            eventBus=new Mock<IEventBus>();
        }

        [Fact]
        public void Cannot_Assing_To_Non_Existing_Project()
        {
            //Arrange

            Mock<IProjectRepository> repo=new Mock<IProjectRepository>();
            repo.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();

            //Act

            AssignUser target = new AssignUser(repo.Object,eventBus.Object,userRepository.Object,null);

            //Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(1, 1));
            Assert.True(ex.Message.Contains("Cannot assign users to project with id:1, because this project non exist."));
        }

        [Fact]
        public void Cannot_Assing_To_Project_With_Deleted_Status()
        {
            //Arrange

            Mock<IProjectRepository> repo = new Mock<IProjectRepository>();
            repo.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Project()
            {
                Status = ProjectStatus.Deleted
            });
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();

            //Act

            AssignUser target = new AssignUser(repo.Object, eventBus.Object,userRepository.Object,null);

            //Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(1, 1));
            Assert.True(ex.Message.Contains("Cannot assign users to project with id:1, because this project non exist."));
        }

        [Fact]
        public void Cannot_Assing_To_Project_Non_existing_user()
        {
            //Arrange

            Mock<IProjectRepository> repo = new Mock<IProjectRepository>();
            repo.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Project()
            {
                Status = ProjectStatus.Active
            });
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            //Act

            AssignUser target = new AssignUser(repo.Object, eventBus.Object, userRepository.Object,null);

            //Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(1, 1));
            Assert.True(ex.Message.Contains("Cannot assign user to project with id:1, because user non exist."));
        }

        [Fact]
        public void Can_Assign_User()
        {
            //Arrange

            Project model = new Project()
            {
                Status = ProjectStatus.Active
            };
            Mock<IProjectRepository> repo = new Mock<IProjectRepository>();
            repo.Setup(m => m.GetById(It.IsAny<int>())).Returns(model);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(m => m.GetById(It.IsAny<int>()))
                .Returns(new User() {Id = 1 , Status = UserStatus.Active});
            Mock<ISetDefaultUserCustomDataBasedOnProject> setDefaultUserCustomDataMock=new Mock<ISetDefaultUserCustomDataBasedOnProject>();

            //Act

            AssignUser target = new AssignUser(repo.Object, eventBus.Object,userRepository.Object,setDefaultUserCustomDataMock.Object);
            target.Invoke(1,1);

            //Assert

            Assert.True(model.ProjectUsers.Any());
            Assert.True(model.ProjectUsers.FirstOrDefault().UserId==1);
        }
    }
}