using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Actions.Implementation
{
    public class GetAllCustomDataFromProjectTest
    {
        [Fact]
        public void Cannot_Get_All_Custom_Data_From_Non_Existing_Project()
        {
            //Arrange

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(() => null);

            //Act

            var target = new GetAllCustomDataFromProject(projectRepositoryMock.Object);

            //Assert

            var result = Assert.Throws<Exception>(() => target.Invoke(1));
            Assert.True(result.Message.Contains("Could not find project with ID:"));
        }

        [Fact]
        public void Cannot_Get_All_Custom_Data_From_Project()
        {
            //Arrange

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>()))
                .Returns(new Project
                {
                    ProjectCustomData = new List<ProjectCustomData>
                    {
                        new ProjectCustomData() ,
                        new ProjectCustomData()
                    }
                });

            //Act

            var target = new GetAllCustomDataFromProject(projectRepositoryMock.Object);
            var result = target.Invoke(1);

            //Assert

            Assert.True(result.Count == 2);
        }
    }
}