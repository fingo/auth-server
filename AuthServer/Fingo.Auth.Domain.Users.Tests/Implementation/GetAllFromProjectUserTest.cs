using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Implementation;
using Fingo.Auth.Domain.Users.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.Users.Tests.Implementation
{
    public class GetAllFromProjectUserTest
    {
        [Fact]
        public void Can_GetAll_Users_With_Status_Registered_And_Active()
        {
            //Arrange

            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(m => m.GetAllUsersFromProject(It.IsAny<int>())).Returns(new List<User>
            {
                new User
                {
                    Id = 1 ,
                    FirstName = "pierwszy" ,
                    LastName = "pierwszy" ,
                    Login = "pierwszy" ,
                    Status = UserStatus.Active
                } ,
                new User
                {
                    Id = 2 ,
                    FirstName = "drugi" ,
                    LastName = "drugi" ,
                    Login = "drugi" ,
                    Status = UserStatus.Registered
                } ,
                new User
                {
                    Id = 3 ,
                    FirstName = "trzeci" ,
                    LastName = "trzeci" ,
                    Login = "trzeci" ,
                    Status = UserStatus.Active
                }
            });

            IGetAllFromProjectUser service = new GetAllFromProjectUser(mockRepository.Object);

            //Act

            var data = service.Invoke(1);

            //Assert

            Assert.True(data.Count() == 3);
        }
    }
}