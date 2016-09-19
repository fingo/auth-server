using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Actions.Implementation
{
    public class DeleteCustomDataFromProjectTest
    {
        [Fact]
        public void Cannot_Delete_CustomData_From_Non_Existing_Project()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();

            Mock<IProjectRepository> projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(() => null);

            // Act

            var target = new DeleteCustomDataFromProject(projectRepositoryMock.Object, eventBusMock.Object);

            // Assert

            var ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(1, "test"));
            Assert.True(ex.Message.Contains("Could not find project with id: "));
        }

        [Fact]
        public void Cannot_Delete_CustomData_From_Project_With_No_Custom_Data()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();

            Mock<IProjectRepository> projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project());

            // Act

            var target = new DeleteCustomDataFromProject(projectRepositoryMock.Object, eventBusMock.Object);

            // Assert

            var ex = Assert.Throws<Exception>(() => target.Invoke(1, "test"));
            Assert.True(ex.Message.Contains("Could not find custom data (name:"));
        }

        [Fact]
        public void Can_Delete_CustomData_From_Project()
        {
            // Arrange

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();

            Project projectMock = new Project()
            {
                ProjectCustomData = new List<ProjectCustomData>()
                {
                    new ProjectCustomData() {ConfigurationName = "test"}
                }
            };

            Mock<IProjectRepository> projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(projectMock);

            // Act

            var target = new DeleteCustomDataFromProject(projectRepositoryMock.Object, eventBusMock.Object);
            target.Invoke(1, "test");

            // Assert

            projectRepositoryMock.Verify(m => m.Edit(projectMock));
            Assert.True(!projectMock.ProjectCustomData.Any());
        }
    }
}