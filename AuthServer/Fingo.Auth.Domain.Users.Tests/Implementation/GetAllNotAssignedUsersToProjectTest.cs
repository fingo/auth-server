using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class GetAllNotAssignedUsersToProjectTest
    {
        [Fact]
        public void Can_Get_All_Users_Non_Assigned_To_Project()
        {
            //Arrange

            var repository = new Mock<IUserRepository>();
            repository.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = 1 ,
                    Status = UserStatus.Active ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 2 , UserId = 1}
                    }
                } ,
                new User
                {
                    Id = 2 ,
                    Status = UserStatus.Active ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 2 , UserId = 2}
                    }
                } ,
                new User
                {
                    Id = 3 ,
                    Status = UserStatus.Active ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 2 , UserId = 3}
                    }
                } ,
                new User
                {
                    Id = 4 ,
                    Status = UserStatus.Active ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 1 , UserId = 4}
                    }
                }
            });

            //Act

            var target = new GetAllNotAssignedUsersToProject(repository.Object);
            var result = target.Invoke(1);

            //Assert

            Assert.True(result.Count() == 3);
        }

        [Fact]
        public void Can_Get_All_Users_Non_Assigned_To_Project_Without_Deleted_Status()
        {
            //Arrange

            var repository = new Mock<IUserRepository>();
            repository.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = 1 ,
                    Status = UserStatus.Active ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 2 , UserId = 1}
                    }
                } ,
                new User
                {
                    Id = 2 ,
                    Status = UserStatus.Deleted ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 2 , UserId = 2}
                    }
                } ,
                new User
                {
                    Id = 3 ,
                    Status = UserStatus.Deleted ,
                    ProjectUsers = new List<ProjectUser>
                    {
                        new ProjectUser {ProjectId = 2 , UserId = 3}
                    }
                }
            });

            //Act

            var target = new GetAllNotAssignedUsersToProject(repository.Object);
            var result = target.Invoke(1);

            //Assert

            Assert.True(result.Count() == 1);
        }
    }
}