using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.UserView;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Actions.Implementation
{
    public class GetUserCustomDataConfigurationViewTest
    {
        [Fact]
        public void Can_Get_Boolean_Custom_Configuration_View()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeUser(ConfigurationType.Boolean , "serializedConfigurationMock"))
                .Returns(new BooleanUserConfiguration
                {
                    Value = true
                });

            var userCustomDataRepositoryMock = new Mock<IUserCustomDataRepository>();
            userCustomDataRepositoryMock.Setup(
                    m => m.GetUserCustomData(It.IsAny<int>() , It.IsAny<int>() , It.IsAny<string>()))
                .Returns(new UserCustomData
                {
                    SerializedConfiguration = "serializedConfigurationMock"
                });

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                Id = 1
            });

            // Act

            var target = new GetUserCustomDataConfigurationView(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object , userCustomDataRepositoryMock.Object);
            var result = target.Invoke(1 , 1 , ConfigurationType.Boolean , "configurationName");

            // Assert

            Assert.IsType<BooleanUserConfigurationView>(result);
            Assert.True(result.ProjectId == 1);
            Assert.True(result.UserId == 1);
            Assert.True(result.ConfigurationName == "configurationName");
            Assert.True(((BooleanUserConfigurationView) result).CurrentValue);
        }

        [Fact]
        public void Can_Get_Number_Custom_Configuration_View()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeUser(ConfigurationType.Number , "serializedConfigurationMock"))
                .Returns(new NumberUserConfiguration
                {
                    Value = 10
                });
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeProject(ConfigurationType.Number , "projectSerializedConfiguration"))
                .Returns(new NumberProjectConfiguration
                {
                    LowerBound = 0 ,
                    UpperBound = 50
                });

            var userCustomDataRepositoryMock = new Mock<IUserCustomDataRepository>();
            userCustomDataRepositoryMock.Setup(
                    m => m.GetUserCustomData(It.IsAny<int>() , It.IsAny<int>() , It.IsAny<string>()))
                .Returns(new UserCustomData
                {
                    SerializedConfiguration = "serializedConfigurationMock"
                });

            var projectCustomDataMock = new ProjectCustomData
            {
                ConfigurationName = "configurationName" ,
                SerializedConfiguration = "projectSerializedConfiguration"
            };
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                Id = 1 ,
                ProjectCustomData = new List<ProjectCustomData>
                {
                    projectCustomDataMock
                }
            });

            // Act

            var target = new GetUserCustomDataConfigurationView(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object , userCustomDataRepositoryMock.Object);
            var result = target.Invoke(1 , 1 , ConfigurationType.Number , "configurationName");

            // Assert

            Assert.IsType<NumberUserConfigurationView>(result);
            Assert.True(result.ProjectId == 1);
            Assert.True(result.UserId == 1);
            Assert.True(result.ConfigurationName == "configurationName");
            Assert.True(((NumberUserConfigurationView) result).CurrentValue == 10);
            Assert.True(((NumberUserConfigurationView) result).LowerBound == 0);
            Assert.True(((NumberUserConfigurationView) result).UpperBound == 50);
        }

        [Fact]
        public void Can_Get_Text_Custom_Configuration_View()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeUser(ConfigurationType.Text , "serializedConfigurationMock"))
                .Returns(new TextUserConfiguration
                {
                    Value = "default"
                });
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeProject(ConfigurationType.Text , "projectSerializedConfiguration"))
                .Returns(new TextProjectConfiguration
                {
                    PossibleValues = new HashSet<string>
                    {
                        "first" ,
                        "second"
                    }
                });

            var userCustomDataRepositoryMock = new Mock<IUserCustomDataRepository>();
            userCustomDataRepositoryMock.Setup(
                    m => m.GetUserCustomData(It.IsAny<int>() , It.IsAny<int>() , It.IsAny<string>()))
                .Returns(new UserCustomData
                {
                    SerializedConfiguration = "serializedConfigurationMock"
                });

            var projectCustomDataMock = new ProjectCustomData
            {
                ProjectId = 1 ,
                ConfigurationName = "configurationName" ,
                SerializedConfiguration = "projectSerializedConfiguration"
            };
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                Id = 1 ,
                ProjectCustomData = new List<ProjectCustomData>
                {
                    projectCustomDataMock
                }
            });

            // Act

            var target = new GetUserCustomDataConfigurationView(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object , userCustomDataRepositoryMock.Object);
            var result = target.Invoke(1 , 1 , ConfigurationType.Text , "configurationName");

            // Assert

            Assert.IsType<TextUserConfigurationView>(result);
            Assert.True(result.ProjectId == 1);
            Assert.True(result.UserId == 1);
            Assert.True(result.ConfigurationName == "configurationName");
            Assert.True(((TextUserConfigurationView) result).CurrentValue == "default");
            Assert.True(((TextUserConfigurationView) result).PossibleValuesList.Count == 3);
        }

        [Fact]
        public void Cannot_Get_Custom_Configuration_View_From_Non_Existing_UserCustomData()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var userCustomDataRepositoryMock = new Mock<IUserCustomDataRepository>();
            userCustomDataRepositoryMock.Setup(
                m => m.GetUserCustomData(It.IsAny<int>() , It.IsAny<int>() , It.IsAny<string>())).Returns(() => null);

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                Id = 1
            });

            // Act

            var target = new GetUserCustomDataConfigurationView(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object , userCustomDataRepositoryMock.Object);

            // Assert

            var ex =
                Assert.Throws<ArgumentNullException>(() => target.Invoke(1 , 1 , ConfigurationType.Boolean , "test"));
            Assert.True(ex.Message.Contains("Could not find user configuration data connected with projectId:"));
        }

        [Fact]
        public void Cannot_Get_Custom_Data_Configuration_View_From_Non_Existing_Project()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var userCustomDataRepositoryMock = new Mock<IUserCustomDataRepository>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(() => null);

            // Act

            var target = new GetUserCustomDataConfigurationView(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object , userCustomDataRepositoryMock.Object);

            // Assert

            var ex =
                Assert.Throws<ArgumentNullException>(() => target.Invoke(1 , 1 , ConfigurationType.Boolean , "test"));
            Assert.True(ex.Message.Contains("Could not find project with id:"));
        }
    }
}