using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Actions.Implementation
{
    public class GetUserCustomDataListFromProjectTest
    {
        [Fact]
        public void Can_Get_Custom_Data_List_From_Project()
        {
            // Arrange

            const string userLogin = "login1";
            const int userId = 1;
            const bool boolValue = true;
            var guid = new Guid();

            var convertService = new CustomDataJsonConvertService();

            var userCustomDataMock = new UserCustomData
            {
                UserId = userId ,
                ProjectCustomData = new ProjectCustomData
                {
                    ProjectId = userId ,
                    ConfigurationName = "testConfigurationName" ,
                    ConfigurationType = ConfigurationType.Boolean
                } ,
                SerializedConfiguration = convertService.Serialize(new BooleanUserConfiguration {Value = boolValue})
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = userId ,
                    Login = userLogin ,
                    UserCustomData = new List<UserCustomData>
                    {
                        userCustomDataMock
                    }
                }
            });

            var projectMock = new Project
            {
                Id = userId ,
                ProjectGuid = guid ,
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        UserCustomData = new List<UserCustomData>
                        {
                            userCustomDataMock
                        }
                    }
                }
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetAll()).Returns(new[] {projectMock});
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(projectMock);

            // Act

            var target = new GetUserCustomDataListFromProject(projectRepositoryMock.Object , userRepositoryMock.Object ,
                convertService);

            var list = target.Invoke(guid , userLogin);

            // Assert

            Assert.Contains(new Tuple<string , string>("testConfigurationName" , boolValue.ToString()) , list);
        }

        [Fact]
        public void Cannot_Get_Custom_Data_List_From_Non_Existing_Project()
        {
            // Arrange

            const string userLogin = "login1";
            const int userId = 1;
            var guid = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = userId ,
                    Login = userLogin
                }
            });

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetAll()).Returns(() => null);

            var convertServiceMock = new Mock<ICustomDataJsonConvertService>();
            // Act

            var target = new GetUserCustomDataListFromProject(projectRepositoryMock.Object , userRepositoryMock.Object ,
                convertServiceMock.Object);

            // Assert

            Assert.Throws<ArgumentNullException>(() => target.Invoke(guid , userLogin));
        }

        [Fact]
        public void Cannot_Get_Custom_Data_List_From_Project_And_Non_Existing_User()
        {
            // Arrange

            const string userLogin = "login1";
            const int userId = 1;
            var guid = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(() => null);

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new Project
                {
                    Id = userId ,
                    ProjectGuid = guid
                }
            });

            var convertServiceMock = new Mock<ICustomDataJsonConvertService>();

            // Act

            var target = new GetUserCustomDataListFromProject(projectRepositoryMock.Object , userRepositoryMock.Object ,
                convertServiceMock.Object);


            // Assert

            Assert.Throws<ArgumentNullException>(() => target.Invoke(guid , userLogin));
        }

        [Fact]
        public void Cannot_Get_Custom_Data_List_From_Project_With_Non_Existing_CustomData_Connected_With_User()
        {
            // Arrange

            const string userLogin = "login1";
            const int userId = 1;
            var guid = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = userId ,
                    Login = userLogin
                }
            });

            var projectMock = new Project
            {
                Id = userId ,
                ProjectGuid = guid ,
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        UserCustomData = new List<UserCustomData>
                        {
                            new UserCustomData
                            {
                                UserId = 2
                            }
                        }
                    }
                }
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetAll()).Returns(new[] {projectMock});
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(projectMock);

            var convertServiceMock = new Mock<ICustomDataJsonConvertService>();

            // Act

            var target = new GetUserCustomDataListFromProject(projectRepositoryMock.Object , userRepositoryMock.Object ,
                convertServiceMock.Object);

            var result = target.Invoke(guid , userLogin);

            // Assert

            Assert.IsType<List<Tuple<string , string>>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Cannot_Get_Custom_Data_List_From_Project_With_Non_Existing_UserCustomData()
        {
            // Arrange

            const string userLogin = "login1";
            const int userId = 1;
            var guid = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(new[]
            {
                new User
                {
                    Id = userId ,
                    Login = userLogin.Insert(0 , "test")
                }
            });

            var projectMock = new Project
            {
                Id = userId ,
                ProjectGuid = guid ,
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        UserCustomData = null
                    }
                }
            };
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetAll()).Returns(new[] {projectMock});
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(projectMock);

            var convertServiceMock = new Mock<ICustomDataJsonConvertService>();

            // Act

            var target = new GetUserCustomDataListFromProject(projectRepositoryMock.Object , userRepositoryMock.Object ,
                convertServiceMock.Object);


            // Assert

            Assert.Throws<ArgumentNullException>(() => target.Invoke(guid , userLogin));
        }
    }
}