using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Implementation
{
    public class GetProjectTest
    {
        [Fact]
        public void Can_GetById_Project()
        {
            //Arrange

            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Project
            {
                Id = 1 ,
                Name = "pierwszy" ,
                Information = new ClientInformation {ContactData = "contactData1"} ,
                Status = ProjectStatus.Active
            });
            IGetProject service = new GetProject(mockRepository.Object);

            //Act

            ProjectDetailModel result = service.Invoke(1);

            //Assert

            Assert.True(result.Id == 1);
            Assert.True(result.Name == "pierwszy");
            Assert.True(result.ContactData == "contactData1");
        }
    }
}