using System;
using System.Collections.Generic;
using Fingo.Auth.AuthServer.Client.Exceptions;
using Fingo.Auth.AuthServer.Client.Services.Implementation;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.JsonWrapper;
using Moq;
using Xunit;

namespace Fingo.Auth.AuthServer.Client.Tests.Services.Implementation
{
    public class RemoteAccountServiceTest
    {
        [Fact]
        public void CreateNewUserInProjectShouldReturnServerConnectionException()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Throws(new Exception());

            IRemoteAccountService remoteAccountService = new RemoteAccountService(postServiceMock.Object);

            // act & assert
            Assert.Throws<ServerConnectionException>(
                () => remoteAccountService.CreateNewUserInProject(new UserModel()));
        }

        [Fact]
        public void CreateNewUserInProjectShouldReturnServerNotValidAnswerException()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Returns("actually not a valid answer");

            IRemoteAccountService remoteAccountService = new RemoteAccountService(postServiceMock.Object);

            // act & assert
            Assert.Throws<ServerNotValidAnswerException>(
                () => remoteAccountService.CreateNewUserInProject(new UserModel()));
        }

        [Fact]
        public void CreateNewUserInProjectShouldSucceed()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Returns(new JsonObject {Result = JsonValues.UserCreatedInProject}.ToJson());

            IRemoteAccountService remoteAccountService = new RemoteAccountService(postServiceMock.Object);

            // act & assert
            remoteAccountService.CreateNewUserInProject(new UserModel());
        }
    }
}