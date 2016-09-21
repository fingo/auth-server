using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Actions.Implementation
{
    public class GetCustomDataConfigurationOrDefaultForProjectTest
    {
        [Fact]
        public void Can_Get_BooleanCustomDataConfiguration_From_Project_When_configurationName_Is_Not_NullOrEmpty()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeProject(ConfigurationType.Boolean , "projectSerializedConfigurationName"))
                .Returns(new BooleanProjectConfiguration
                {
                    Default = true
                });

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        ConfigurationName = "projectConfigurationName" ,
                        SerializedConfiguration = "projectSerializedConfigurationName"
                    }
                }
            });

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Invoke(1 , "projectConfigurationName" , ConfigurationType.Boolean);

            // Assert

            Assert.IsType<BooleanProjectConfiguration>(result);
            Assert.True(((BooleanProjectConfiguration) result).Default);
        }

        [Fact]
        public void Can_Get_New_BooleanCustomDataConfiguration_From_Project_When_configurationName_Is_NullOrEmpty()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project());

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Invoke(1 , null , ConfigurationType.Boolean);

            // Assert

            Assert.IsType<BooleanProjectConfiguration>(result);
            Assert.True(((BooleanProjectConfiguration) result).Default == default(bool));
        }

        [Fact]
        public void Can_Get_New_NumberCustomDataConfiguration_From_Project_When_configurationName_Is_NullOrEmpty()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project());

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Invoke(1 , null , ConfigurationType.Number);

            // Assert

            Assert.IsType<NumberProjectConfiguration>(result);
            Assert.True(((NumberProjectConfiguration) result).Default == default(int));
            Assert.True(((NumberProjectConfiguration) result).UpperBound == default(int));
            Assert.True(((NumberProjectConfiguration) result).LowerBound == default(int));
        }

        [Fact]
        public void Can_Get_New_TextCustomDataConfiguration_From_Project_When_configurationName_Is_NullOrEmpty()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project());

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Invoke(1 , null , ConfigurationType.Text);

            // Assert

            Assert.IsType<TextProjectConfiguration>(result);
            Assert.True(((TextProjectConfiguration) result).Default == default(string));
            Assert.True(((TextProjectConfiguration) result).PossibleValues.Count == 0);
        }

        [Fact]
        public void Can_Get_NumberCustomDataConfiguration_From_Project_When_configurationName_Is_Not_NullOrEmpty()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeProject(ConfigurationType.Number , "projectSerializedConfigurationName"))
                .Returns(new NumberProjectConfiguration
                {
                    Default = 5 ,
                    LowerBound = 0 ,
                    UpperBound = 10
                });

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        ConfigurationName = "projectConfigurationName" ,
                        SerializedConfiguration = "projectSerializedConfigurationName"
                    }
                }
            });

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Invoke(1 , "projectConfigurationName" , ConfigurationType.Number);

            // Assert

            Assert.IsType<NumberProjectConfiguration>(result);
            Assert.True(((NumberProjectConfiguration) result).Default == 5);
            Assert.True(((NumberProjectConfiguration) result).LowerBound == 0);
            Assert.True(((NumberProjectConfiguration) result).UpperBound == 10);
        }

        [Fact]
        public void Can_Get_TextCustomDataConfiguration_From_Project_When_configurationName_Is_Not_NullOrEmpty()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                    m => m.DeserializeProject(ConfigurationType.Text , "projectSerializedConfigurationName"))
                .Returns(new TextProjectConfiguration
                {
                    Default = "default" ,
                    PossibleValues = new HashSet<string>
                    {
                        "val1" ,
                        "val2"
                    }
                });

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        ConfigurationName = "projectConfigurationName" ,
                        SerializedConfiguration = "projectSerializedConfigurationName"
                    }
                }
            });

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);
            var result = target.Invoke(1 , "projectConfigurationName" , ConfigurationType.Text);

            // Assert

            Assert.IsType<TextProjectConfiguration>(result);
            Assert.True(((TextProjectConfiguration) result).Default == "default");
            Assert.True(((TextProjectConfiguration) result).PossibleValues.Count == 2);
        }

        [Fact]
        public void Cannot_Get_CustomDataConfiguration_From_Non_Existing_Project()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(() => null);

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);

            // Assert

            var ex = Assert.Throws<ArgumentNullException>(() => target.Invoke(1 , "test" , ConfigurationType.Boolean));
            Assert.True(ex.Message.Contains("Could not find project with id:"));
        }

        [Fact]
        public void
            Cannot_Get_CustomDataConfiguration_From_Project_When_configurationName_Is_Not_NullOrEmpty_And_jsonConvertService_Throw_exception
            ()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();
            customDataJsonConvertServiceMock.Setup(
                m => m.DeserializeProject(It.IsAny<ConfigurationType>() , It.IsAny<string>())).Throws(new Exception());

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project
            {
                ProjectCustomData = new List<ProjectCustomData>
                {
                    new ProjectCustomData
                    {
                        ConfigurationName = "projectConfigurationName"
                    }
                }
            });

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);

            // Assert

            var ex =
                Assert.Throws<Exception>(() => target.Invoke(1 , "projectConfigurationName" , ConfigurationType.Boolean));
            Assert.True(
                ex.Message.Contains("There was a problem with deserializing policy configurations of project with id:"));
        }

        [Fact]
        public void
            Cannot_Get_CustomDataConfiguration_From_Project_When_configurationName_Is_Not_NullOrEmpty_And_Project_Dont_Have_Custom_Data
            ()
        {
            // Arrange

            var customDataJsonConvertServiceMock = new Mock<ICustomDataJsonConvertService>();

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(m => m.GetByIdWithCustomDatas(It.IsAny<int>())).Returns(new Project());

            // Act

            var target = new GetCustomDataConfigurationOrDefaultForProject(projectRepositoryMock.Object ,
                customDataJsonConvertServiceMock.Object);

            // Assert

            var ex = Assert.Throws<Exception>(() => target.Invoke(1 , "test" , ConfigurationType.Boolean));
            Assert.True(ex.Message.Contains("Something went wrong in GetCustomDataConfigurationOrDefaultForProject"));
        }
    }
}