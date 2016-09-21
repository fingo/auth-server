using System;
using System.Linq.Expressions;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Interfaces.CustomData;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Moq;
using Xunit;

namespace Fingo.Auth.Domain.CustomData.Tests.Factories.Actions.Implementation
{
    public class SaveUserCustomDataTest
    {
        [Fact]
        public void Can_Save_Boolean_User_Custom_Data()
        {
            // Arrange

            var booleanConfiguration = new BooleanUserConfiguration {Value = true};
            var result = new UserCustomData
            {
                Id = 1 ,
                ProjectCustomData = new ProjectCustomData
                {
                    ConfigurationName = "firstConfiguration"
                }
            };

            var customServiceMock = new Mock<ICustomDataJsonConvertService>();
            customServiceMock.Setup(m => m.Serialize(booleanConfiguration)).Returns(booleanConfiguration.Value.ToString);

            var userCustomDataRepository = new Mock<IUserCustomDataRepository>();
            userCustomDataRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<UserCustomData , bool>>>())).Returns(
                new[]
                {
                    result
                });

            // Act

            var target = new SaveUserCustomData(userCustomDataRepository.Object , customServiceMock.Object);
            target.Invoke(1 , 1 , "name" , booleanConfiguration);

            // Assert

            Assert.True(result.ModificationDate.Date == DateTime.UtcNow.Date);
            Assert.True(result.SerializedConfiguration == true.ToString());
            userCustomDataRepository.Verify(m => m.Edit(result));
        }

        [Fact]
        public void Can_Save_Number_User_Custom_Data()
        {
            // Arrange

            var numberUserConfiguration = new NumberUserConfiguration {Value = 10};
            var result = new UserCustomData
            {
                Id = 1 ,
                ProjectCustomData = new ProjectCustomData
                {
                    ConfigurationName = "firstConfiguration"
                }
            };

            var customServiceMock = new Mock<ICustomDataJsonConvertService>();
            customServiceMock.Setup(m => m.Serialize(numberUserConfiguration))
                .Returns(numberUserConfiguration.Value.ToString);

            var userCustomDataRepository = new Mock<IUserCustomDataRepository>();
            userCustomDataRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<UserCustomData , bool>>>())).Returns(
                new[]
                {
                    result
                });

            // Act

            var target = new SaveUserCustomData(userCustomDataRepository.Object , customServiceMock.Object);
            target.Invoke(1 , 1 , "name" , numberUserConfiguration);

            // Assert

            Assert.True(result.ModificationDate.Date == DateTime.UtcNow.Date);
            Assert.True(result.SerializedConfiguration == 10.ToString());
            userCustomDataRepository.Verify(m => m.Edit(result));
        }

        [Fact]
        public void Can_Save_Text_User_Custom_Data()
        {
            // Arrange

            var textUserConfiguration = new TextUserConfiguration {Value = "test"};
            var result = new UserCustomData
            {
                Id = 1 ,
                ProjectCustomData = new ProjectCustomData
                {
                    ConfigurationName = "firstConfiguration"
                }
            };

            var customServiceMock = new Mock<ICustomDataJsonConvertService>();
            customServiceMock.Setup(m => m.Serialize(textUserConfiguration))
                .Returns(textUserConfiguration.Value.ToString);

            var userCustomDataRepository = new Mock<IUserCustomDataRepository>();
            userCustomDataRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<UserCustomData , bool>>>())).Returns(
                new[]
                {
                    result
                });

            // Act

            var target = new SaveUserCustomData(userCustomDataRepository.Object , customServiceMock.Object);
            target.Invoke(1 , 1 , "name" , textUserConfiguration);

            // Assert

            Assert.True(result.ModificationDate.Date == DateTime.UtcNow.Date);
            Assert.True(result.SerializedConfiguration == "test");
            userCustomDataRepository.Verify(m => m.Edit(result));
        }
    }
}