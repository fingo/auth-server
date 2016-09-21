using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Implementation
{
    public class UpdateProjectTest
    {
        [Fact]
        public void Can_Update_Project()
        {
            //Arrange
            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Project
            {
                Id = 1 ,
                Name = "pierwszy" ,
                ProjectGuid = Guid.Parse("e3491b73-1f7b-4afb-b031-28b84c5ea4e2") ,
                Information = new ClientInformation {ContactData = "contactData1"} ,
                Status = ProjectStatus.Active
            });
            IUpdateProject service = new UpdateProject(mockRepository.Object);

            //Act

            service.Invoke(1 , "newName1");
            var afterUpdate = mockRepository.Object.GetById(4);

            //Assert

            Assert.True(afterUpdate.Name == "newName1");
            Assert.True(afterUpdate.ProjectGuid != Guid.Parse("e3491b73-1f7b-4afb-b031-28b84c5ea4e2"));
        }
    }
}