using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Projects.Implementation;
using Fingo.Auth.Domain.Projects.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Projects.Tests.Implementation
{
    public class GetAllProjectsTest
    {
        [Fact]
        public void Can_GetAll_Project()
        {
            //Arrange

            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(m => m.GetAll()).Returns(new[]
            {
                new Project
                {
                    Id = 1 ,
                    Name = "pierwszy" ,
                    Status = ProjectStatus.Active
                } ,
                new Project
                {
                    Id = 2 ,
                    Name = "drugi" ,
                    Status = ProjectStatus.Active
                }
            });
            IGetAllProjects service = new GetAllProjects(mockRepository.Object);

            //Act

            var data = service.Invoke();

            //Assert

            Assert.True(data.Count() == 2);
            Assert.True(data.First().Name == "pierwszy");
        }
    }
}