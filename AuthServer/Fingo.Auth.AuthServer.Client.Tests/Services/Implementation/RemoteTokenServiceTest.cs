using System;
using System.Collections.Generic;
using Fingo.Auth.AuthServer.Client.Exceptions;
using Fingo.Auth.AuthServer.Client.Services.Implementation;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.JsonWrapper;
using Moq;
using Xunit;

namespace Fingo.Auth.AuthServer.Client.Tests.Services.Implementation
{
    public class RemoteTokenServiceTest
    {
        [Fact]
        public void AcquireTokenShouldReturnNotValidServerAnswerException()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Returns("actually not a valid answer");

            IRemoteTokenService remoteTokenService = new RemoteTokenService(postServiceMock.Object);

            // act & assert
            Assert.Throws<ServerNotValidAnswerException>(
                () => remoteTokenService.AcquireToken("sample_login" , "sample_password"));
        }

        [Fact]
        public void AcquireTokenShouldReturnSampleJwt()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Returns(new JsonObject
                {
                    Result = JsonValues.Authenticated ,
                    Jwt = "sample_jwt"
                }.ToJson());

            IRemoteTokenService remoteTokenService = new RemoteTokenService(postServiceMock.Object);
            // act
            var answer = remoteTokenService.AcquireToken("sample_login" , "sample_password");

            // assert
            Assert.Equal("sample_jwt" , answer);
        }

        [Fact]
        public void AcquireTokenShouldThrowServerConnectionException()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Throws(new Exception());

            IRemoteTokenService remoteTokenService = new RemoteTokenService(postServiceMock.Object);

            // act & assert
            Assert.Throws<ServerConnectionException>(
                () => remoteTokenService.AcquireToken("sample_login" , "sample_password"));
        }

        [Fact]
        public void VerifyTokenShouldReturnFalseBecauseOfException()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Throws(new Exception());

            IRemoteTokenService remoteTokenService = new RemoteTokenService(postServiceMock.Object);

            // act
            var answer = remoteTokenService.VerifyToken("sample_jwt");

            // assert
            Assert.Equal(false , answer);
        }

        [Fact]
        public void VerifyTokenShouldReturnFalseBecauseOfNotValidServerAnswer()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Returns("actually not a valid answer");

            IRemoteTokenService remoteTokenService = new RemoteTokenService(postServiceMock.Object);
            // act
            var answer = remoteTokenService.VerifyToken("sample_jwt");

            // assert
            Assert.Equal(false , answer);
        }

        [Fact]
        public void VerifyTokenShouldReturnTrue()
        {
            // arrange
            var postServiceMock = new Mock<IPostService>();
            postServiceMock.Setup(
                    serv => serv.SendAndGetAnswer(It.IsAny<string>() , It.IsAny<Dictionary<string , string>>()))
                .Returns(new JsonObject
                {
                    Result = JsonValues.TokenValid
                }.ToJson());

            IRemoteTokenService remoteTokenService = new RemoteTokenService(postServiceMock.Object);

            // act
            var answer = remoteTokenService.VerifyToken("sample_jwt");

            // assert
            Assert.Equal(true , answer);
        }
    }
}