using System.Collections.Generic;
using System.Linq;
using System.Net;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.Policies;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Factories.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Services.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.ManagementApp.Controllers;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace Fingo.Auth.ManagementApp.Tests.Controllers
{
    public class ProjectsControllerTests
    {
        public ProjectsControllerTests()
        {
            eventBus = new Mock<IEventBus>();
            eventWatcher = new Mock<IEventWatcher>();
            logger = new Mock<ILogger<ProjectsController>>();
        }

        private readonly Mock<IEventBus> eventBus;
        private readonly Mock<IEventWatcher> eventWatcher;
        private readonly Mock<ILogger<ProjectsController>> logger;

        [Fact]
        public void Can_Add_New_Project()
        {
            //Arrange

            var tempDataMock = new Mock<ITempDataDictionary>();

            var projectRepositoryMock = new Mock<IProjectRepository>();

            var factoryMock = new Mock<IAddProjectFactory>();
            factoryMock.Setup(m => m.Create()).Returns(new AddProject(projectRepositoryMock.Object , eventBus.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                null ,
                factoryMock.Object , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetResult = target.Add("nowy");

            //Assert

            projectRepositoryMock.Verify(m => m.Add(It.IsAny<Project>()));
            Assert.True(targetResult.StatusCode == HttpStatusCode.NoContent);
        }

        [Fact]
        public void Can_Assign_No_Users_To_Project()
        {
            //Arrange

            var model = new Project {Id = 1 , Status = ProjectStatus.Active , Name = "first"};

            var tempDataMock = new Mock<ITempDataDictionary>();
            var messageSenderMock = new Mock<IMessageSender>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(model);
            projectRepositoryMock.Setup(m => m.GetAllUsersFromProject(It.IsAny<int>())).Returns(new List<User>());

            var userRepositoryMock = new Mock<IUserRepository>();

            var projectMock = new Mock<IGetProjectFactory>();
            projectMock.Setup(m => m.Create()).Returns(new GetProject(projectRepositoryMock.Object));

            var setDefaultUserCustomDataMock = new Mock<ISetDefaultUserCustomDataBasedOnProject>();

            var getUserFactoryMock = new Mock<IGetUserFactory>();

            var getProjectFactoryMock = new Mock<IGetProjectWithAllFactory>();
            getProjectFactoryMock.Setup(m => m.Create()).Returns(new GetProjectWithAll(projectRepositoryMock.Object));

            var factoryMock = new Mock<IAssignUserFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new AssignUser(projectRepositoryMock.Object , eventBus.Object , userRepositoryMock.Object ,
                    setDefaultUserCustomDataMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null ,
                projectMock.Object , null ,
                null , null , factoryMock.Object , messageSenderMock.Object , getUserFactoryMock.Object ,
                getProjectFactoryMock.Object , null);
            target.TempData = tempDataMock.Object;

            var targetResult = (RedirectToActionResult) target.AsignUsersToProject(1 , new int[] {});

            //Assert

            Assert.True(targetResult.ActionName == "GetById");
            Assert.True((int) targetResult.RouteValues["id"] == 1);
            Assert.True(model.ProjectUsers == null);
        }

        [Fact]
        public void Can_Assign_Users_To_Project()
        {
            //Arrange

            var model = new Project {Id = 1 , Status = ProjectStatus.Active , Name = "first"};

            var tempDataMock = new Mock<ITempDataDictionary>();
            var messageSenderMock = new Mock<IMessageSender>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(model);
            projectRepositoryMock.Setup(m => m.GetAllUsersFromProject(It.IsAny<int>())).Returns(new List<User>());

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(m => m.GetById(1))
                .Returns(new User {Id = 1 , Status = UserStatus.Active});
            userRepository.Setup(m => m.GetById(2))
                .Returns(new User {Id = 2 , Status = UserStatus.Active});

            var getUserFactoryMock = new Mock<IGetUserFactory>();

            var getProjectFactoryMock = new Mock<IGetProjectWithAllFactory>();
            getProjectFactoryMock.Setup(m => m.Create()).Returns(new GetProjectWithAll(projectRepositoryMock.Object));

            var projectMock = new Mock<IGetProjectFactory>();
            projectMock.Setup(m => m.Create()).Returns(new GetProject(projectRepositoryMock.Object));

            var setDefaultUserCustomDataMock = new Mock<ISetDefaultUserCustomDataBasedOnProject>();

            var factoryMock = new Mock<IAssignUserFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new AssignUser(projectRepositoryMock.Object , eventBus.Object , userRepository.Object ,
                    setDefaultUserCustomDataMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null ,
                projectMock.Object , null ,
                null , null , factoryMock.Object , messageSenderMock.Object , getUserFactoryMock.Object ,
                getProjectFactoryMock.Object , null);
            target.TempData = tempDataMock.Object;

            var targetResult = (RedirectToActionResult) target.AsignUsersToProject(1 , new[] {1 , 2});

            //Assert

            Assert.True(targetResult.ActionName == "GetById");
            Assert.True((int) targetResult.RouteValues["id"] == 1);
            Assert.True(model.ProjectUsers.Count() == 2);
        }

        [Fact]
        public void Can_Delete_User_From_Project()
        {
            var user = new User
            {
                Id = 1 ,
                Login = "first" ,
                ProjectUsers = new List<ProjectUser>
                {
                    new ProjectUser {ProjectId = 1}
                } ,
                Status = UserStatus.Active
            };
            var eventBusMock = new Mock<IEventBus>();
            var tempDataMock = new Mock<ITempDataDictionary>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(user);

            var factoryMock = new Mock<IDeleteByIdUserFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new DeleteByIdUser(userRepositoryMock.Object , eventBusMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object ,
                factoryMock.Object , null , null ,
                null , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetView = target.DeleteUserFromProject(1 , 1);

            //Assert

            userRepositoryMock.Verify(m => m.Edit(It.IsAny<User>()));
            Assert.True(targetView.StatusCode == HttpStatusCode.NoContent);
            Assert.True(user.Status == UserStatus.Active);
        }

        [Fact]
        public void Can_Delete_Valid_Project()
        {
            //Arrange

            var model = new Project {Id = 1 , Status = ProjectStatus.Active , Name = "first"};

            var eventBusMock = new Mock<IEventBus>();
            var tempDataMock = new Mock<ITempDataDictionary>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithAll(It.IsAny<int>())).Returns(model);

            var factoryMock = new Mock<IDeleteProjectFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new DeleteProject(projectRepositoryMock.Object , eventBusMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                factoryMock.Object ,
                null , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetResult = target.Delete(1);

            //Assert

            Assert.True(targetResult.StatusCode == HttpStatusCode.NoContent);
            Assert.True(model.Status == ProjectStatus.Deleted);
        }

        [Fact]
        public void Can_Get_All_Projects()
        {
            var projectMock = new Mock<IGetAllProjects>();
            projectMock.Setup(m => m.Invoke()).Returns(new[]
            {
                new ProjectModel {Id = 1 , Name = "first"} ,
                new ProjectModel {Id = 2 , Name = "second"} ,
                new ProjectModel {Id = 3 , Name = "third"}
            });

            var factoryMock = new Mock<IGetAllProjectFactory>();
            factoryMock.Setup(m => m.Create()).Returns(projectMock.Object);

            //Act

            var target =
                (ViewResult)
                new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null , null ,
                    null , factoryMock.Object , null , null , null , null , null).All();

            //Assert

            Assert.True(target.ViewName == null);
            Assert.True(target.Model == null);
        }

        [Fact]
        public void Can_Get_Project_By_Id()
        {
            //Arrange

            var repositoryMock = new Mock<IProjectRepository>();
            repositoryMock.Setup(m => m.GetById(1)).Returns(new Project
            {
                Id = 1 ,
                Name = "first" ,
                Status = ProjectStatus.Active ,
                ProjectPolicies = new List<ProjectPolicies>
                {
                    new ProjectPolicies {Policy = Policy.AccountExpirationDate} ,
                    new ProjectPolicies {Policy = Policy.ExcludeCommonPasswords}
                }
            });
            repositoryMock.Setup(m => m.GetAllUsersFromProject(1)).Returns(new[]
            {
                new User {Id = 1} ,
                new User {Id = 2}
            });
            repositoryMock.Setup(m => m.GetByIdWithCustomDatas(1)).Returns(new Project
            {
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData {ProjectId = 1 , ConfigurationName = "configurationName1"} ,
                    new ProjectCustomData {ProjectId = 1 , ConfigurationName = "configurationName2"}
                }
            });


            var factoryMock = new Mock<IGetProjectWithAllFactory>();
            factoryMock.Setup(m => m.Create()).Returns(new GetProjectWithAll(repositoryMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                null ,
                null , null , null , null , null , factoryMock.Object , null);

            var targetView = (ViewResult) target.GetById(1);
            var modelResult = (ProjectDetailWithAll) targetView.ViewData.Model;

            //Assert

            Assert.True(modelResult.Id == 1);
            Assert.True(modelResult.Name == "first");
            Assert.True(modelResult.Users.Count() == 2);
            Assert.True(modelResult.Data.Count() == 2);
            Assert.True(modelResult.Policies.Count() == 2);
        }

        [Fact]
        public void Cannot_Add_New_Project_With_Empty_Name()
        {
            //Arrange

            var tempDataMock = new Mock<ITempDataDictionary>();

            var projectRepositoryMock = new Mock<IProjectRepository>();

            var factoryMock = new Mock<IAddProjectFactory>();
            factoryMock.Setup(m => m.Create()).Returns(new AddProject(projectRepositoryMock.Object , eventBus.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                null ,
                factoryMock.Object , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetResult = target.Add("");

            //Assert

            Assert.True(targetResult.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Cannot_Assign_Null_As_Users_To_Project()
        {
            //Arrange

            var model = new Project {Id = 1 , Status = ProjectStatus.Active , Name = "first"};

            var tempDataMock = new Mock<ITempDataDictionary>();
            var messageSenderMock = new Mock<IMessageSender>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(model);
            projectRepositoryMock.Setup(m => m.GetAllUsersFromProject(It.IsAny<int>())).Returns(new List<User>());

            var usersRepository = new Mock<IUserRepository>();

            var getUserFactoryMock = new Mock<IGetUserFactory>();

            var getProjectFactoryMock = new Mock<IGetProjectWithAllFactory>();
            getProjectFactoryMock.Setup(m => m.Create()).Returns(new GetProjectWithAll(projectRepositoryMock.Object));

            var factoryMock = new Mock<IAssignUserFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new AssignUser(projectRepositoryMock.Object , eventBus.Object , usersRepository.Object , null));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                null ,
                null , null , factoryMock.Object , messageSenderMock.Object , getUserFactoryMock.Object ,
                getProjectFactoryMock.Object , null);
            target.TempData = tempDataMock.Object;

            var targetResult = (ViewResult) target.AsignUsersToProject(1 , null);

            //Assert

            Assert.True(targetResult.ViewName == "ErrorPage");
        }

        [Fact]
        public void Cannot_Assign_Users_To_Non_Existing_Project()
        {
            //Arrange
            var eventBusMock = new Mock<IEventBus>();
            var tempDataMock = new Mock<ITempDataDictionary>();
            var messageSenderMock = new Mock<IMessageSender>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            var getProjectFactoryMock = new Mock<IGetProjectWithAllFactory>();
            getProjectFactoryMock.Setup(m => m.Create()).Returns(new GetProjectWithAll(projectRepositoryMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                null ,
                null , null , null , messageSenderMock.Object , null , getProjectFactoryMock.Object , null);
            target.TempData = tempDataMock.Object;

            var targetResult = (ViewResult) target.AsignUsersToProject(1 , new[] {1 , 2});

            //Assert

            Assert.True(targetResult.ViewName == "ErrorPage");
        }

        [Fact]
        public void Cannot_Delete_Non_Existed_Project()
        {
            //Arrange

            var tempDataMock = new Mock<ITempDataDictionary>();
            var eventBusMock = new Mock<IEventBus>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            var factoryMock = new Mock<IDeleteProjectFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new DeleteProject(projectRepositoryMock.Object , eventBusMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                factoryMock.Object ,
                null , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetResult = target.Delete(1);

            //Assert

            Assert.True(targetResult.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Cannot_Delete_Non_Existed_User_From_Project()
        {
            //Arrange

            var eventBusMock = new Mock<IEventBus>();
            var tempDataMock = new Mock<ITempDataDictionary>();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            var factoryMock = new Mock<IDeleteByIdUserFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new DeleteByIdUser(userRepositoryMock.Object , eventBusMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object ,
                factoryMock.Object , null , null ,
                null , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetView = target.DeleteUserFromProject(1 , 1);

            //Assert

            Assert.True(targetView.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Cannot_Delete_Project_With_Deleted_Status()
        {
            //Arrange

            var model = new Project {Id = 1 , Status = ProjectStatus.Deleted , Name = "first"};

            var tempDataMock = new Mock<ITempDataDictionary>();
            var eventBusMock = new Mock<IEventBus>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(model);

            var factoryMock = new Mock<IDeleteProjectFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new DeleteProject(projectRepositoryMock.Object , eventBusMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                factoryMock.Object ,
                null , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetResult = target.Delete(1);

            //Assert

            Assert.True(targetResult.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Cannot_Delete_User_From_Non_Existed_Project()
        {
            var user = new User
            {
                Id = 1 ,
                Login = "first" ,
                ProjectUsers = new List<ProjectUser>
                {
                    new ProjectUser {ProjectId = 1}
                } ,
                Status = UserStatus.Active
            };
            var eventBusMock = new Mock<IEventBus>();
            var tempDataMock = new Mock<ITempDataDictionary>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(user);

            var factoryMock = new Mock<IDeleteByIdUserFactory>();
            factoryMock.Setup(m => m.Create())
                .Returns(new DeleteByIdUser(userRepositoryMock.Object , eventBusMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object ,
                factoryMock.Object , null , null ,
                null , null , null , null , null , null , null);
            target.TempData = tempDataMock.Object;

            var targetView = target.DeleteUserFromProject(2 , 1);

            //Assert

            Assert.True(targetView.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Cannot_Get_Non_Existed_Project_By_Id()
        {
            //Arrange

            var tempDataMock = new Mock<ITempDataDictionary>();
            var repositoryMock = new Mock<IProjectRepository>();
            repositoryMock.Setup(m => m.GetById(1)).Returns(new Project
            {
                Id = 1 ,
                Name = "first"
            });

            var factoryMock = new Mock<IGetProjectWithAllFactory>();
            factoryMock.Setup(m => m.Create()).Returns(new GetProjectWithAll(repositoryMock.Object));

            //Act

            var target = new ProjectsController(eventWatcher.Object , logger.Object , eventBus.Object , null , null ,
                null ,
                null , null , null , null , null , factoryMock.Object , null);
            target.TempData = tempDataMock.Object;
            var targetView = (ViewResult) target.GetById(2);

            var viewResultName = targetView.ViewName;

            //Assert

            Assert.True(viewResultName == "ErrorPage");
        }
    }
}