using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Fingo.Auth.DbAccess.Models.Policies;
using Moq;
using Xunit;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Policies.Enums;

namespace Fingo.Auth.Domain.Projects.Tests.Implementation
{
    public class DeleteProjectTest
    {
        [Fact]
        public void Can_Delete_Project()
        {
            //Arrange

            Mock<IProjectRepository> mockRepository=new Mock<IProjectRepository>();
            Project mockProject = new Project()
            {
                Id = 1,
                Name = "pierwszy",
                Status = ProjectStatus.Active,
                ModificationDate = DateTime.Now.AddDays(-1),
                ProjectPolicies = new List<ProjectPolicies>()                
             };

            mockProject.ProjectPolicies.Add(new ProjectPolicies
            {
                Policy = Policy.MinimumPasswordLength
            });


            mockRepository.Setup(m => m.GetByIdWithAll(It.IsAny<int>())).Returns(mockProject);          

            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();

            IDeleteProject service = new DeleteProject(mockRepository.Object,eventBusMock.Object);

            //Act

            service.Invoke(1);
            Project removedProject = mockRepository.Object.GetByIdWithAll(1);

            //Assert
            mockRepository.Verify(m => m.Edit(mockProject));
           
            Assert.True(removedProject != null);
            Assert.True(removedProject.Status == ProjectStatus.Deleted);
            Assert.True(removedProject.Id==1);
            Assert.True(removedProject.Name=="pierwszy");
            Assert.True(removedProject.ProjectPolicies == null);
        }

        [Fact]
        public void Cannot_Delete_Non_Existing_Project()
        {
            //Arrange
            Mock<IEventBus> eventBusMock = new Mock<IEventBus>();
            Mock<IProjectRepository> mockRepository = new Mock<IProjectRepository>();
            Project mockProject = new Project()
            {
                Id = 1,
                Name = "pierwszy",
                Status = ProjectStatus.Active
            };
            mockRepository.Setup(m => m.GetByIdWithAll(1)).Returns(mockProject);

            IDeleteProject service = new DeleteProject(mockRepository.Object, eventBusMock.Object);

            //Act

            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => service.Invoke(2));

            //Assert
           
            Assert.True(exc.Message.Contains("Cannot find project with id=2"));
        }
    }
}