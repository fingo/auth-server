using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Implementation
{
    public class AddProjectTest
    {
        [Fact]
        public void Can_Add_Project_With_Status_Active()
        {
            //Arrange

            Mock<IEventBus> eventMock=new Mock<IEventBus>();
            Mock<IProjectRepository> mockRepository=new Mock<IProjectRepository>();
            mockRepository.Setup(m => m.GetAll()).Returns(() => null);
            mockRepository.Setup(m => m.Add(It.IsAny<Project>()))
                .Callback((Project p) => mockRepository.Setup(m => m.GetAll()).Returns(new[] { p }));
            IAddProject service = new AddProject(mockRepository.Object,eventMock.Object);

            Project newProject = new Project()
            {
                Name = "newProject"
            };

            ProjectModel modelProject=new ProjectModel(newProject);

            //Act

            service.Invoke(modelProject);
            var allProjects = mockRepository.Object.GetAll();
            var addedProject = allProjects.Last();

            //Assert

            Assert.True(newProject.Name == addedProject.Name);
            Assert.True(allProjects.Count()==1);
            Assert.True(addedProject.Status == ProjectStatus.Active);
        }
    }
}