using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fingo.Auth.AuthServer.Services;
using Fingo.Auth.AuthServer.Services.Implementation;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.JsonWrapper;
using Newtonsoft.Json;
using Xunit;
using Moq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Interfaces;

namespace Fingo.Auth.AuthServer.Tests.Services.Implementation
{
    public class TokenServiceTest
    {
        private readonly ILogger<TokenService> _logger;

        private string _jwtValid;
        private string _jwtValidButExpired;
        private string _jwtSignedWithWrongKey;
        private Guid _sampleGuid;

        public TokenServiceTest()
        {
            _logger = new Logger<TokenService>();
        }

        [Fact]
        public void AcquireTokenShouldReturnAuthenticated()
        {
            // arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.FindBy(It.IsAny<Expression<Func<Project, bool>>>()))
                                 .Returns(new[] { new Project { ProjectGuid = _sampleGuid, Status = ProjectStatus.Active } });

            projectRepositoryMock.Setup(repo => repo.GetByGuid(It.IsAny<Guid>()))
                     .Returns(new Project { ProjectGuid = _sampleGuid, Status = ProjectStatus.Active, Id = 1 });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                                .Returns(new[] { new User { Login = "login", Password = "password", Status = UserStatus.Active } });

            var getPoliciesFromProjectActionMock = new Mock<IGetPoliciesFromProject>();
            var getPoliciesFromProjectFactoryMock = new Mock<IGetPoliciesFromProjectFactory>();

            getPoliciesFromProjectFactoryMock.Setup(fac => fac.Create()).Returns(getPoliciesFromProjectActionMock.Object);
            getPoliciesFromProjectActionMock.Setup(ac => ac.Invoke(It.IsAny<int>()))
                .Returns(new List<Tuple<Policy, PolicyConfiguration>>());

            var hashingServiceMock = new Mock<IHashingService>();
            hashingServiceMock.Setup(hs => hs.HashWithDatabaseSalt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("password");

            var getUserCustomDataFactoryMock = new Mock<IGetUserCustomDataListFromProjectFactory>();
            var getUserCustomDataActionMock = new Mock<IGetUserCustomDataListFromProject>();

            getUserCustomDataFactoryMock.Setup(fac => fac.Create()).Returns(getUserCustomDataActionMock.Object);
            getUserCustomDataActionMock.Setup(ac => ac.Invoke(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new List<Tuple<string, string>>());

            var getUserPolicyConfigurationMock = new Mock<IGetUserPolicyConfigurationOrDefaultFromProjectFactory>();

            var jwtLibraryWrapperMock = new Mock<IJwtLibraryWrapperService>();
            jwtLibraryWrapperMock.Setup(lib => lib.Encode(It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()))
                .Returns("a.b.c");

            var tokenService = new TokenService(userRepositoryMock.Object, projectRepositoryMock.Object,
                jwtLibraryWrapperMock.Object, _logger, getPoliciesFromProjectFactoryMock.Object,
                hashingServiceMock.Object, getUserCustomDataFactoryMock.Object, getUserPolicyConfigurationMock.Object);

            // act
            string jsonResult = tokenService.AcquireToken("login", "password", _sampleGuid);
            var deserialized = JsonConvert.DeserializeObject<JsonObject>(jsonResult);

            // assert
            Assert.Equal(JsonValues.Authenticated, deserialized.Result);
            Assert.NotNull(deserialized.Jwt);
            Assert.Null(deserialized.ErrorDetails);
        }

        [Fact]
        public void VerifyTokenShouldReturnTokenValid()
        {
            // arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.FindBy(It.IsAny<Expression<Func<Project, bool>>>()))
                                 .Returns(new[] { new Project { ProjectGuid = _sampleGuid, Status = ProjectStatus.Active } });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                                .Returns(new[] { new User { Login = "login", Password = "password", Status = UserStatus.Active } });

            var jwtLibraryWrapperMock = new Mock<IJwtLibraryWrapperService>();
            jwtLibraryWrapperMock.Setup(lib => lib.Decode(It.IsAny<string>(), It.IsAny<string>())).Returns(DecodeResult.TokenValid);

            var tokenService = new TokenService(userRepositoryMock.Object, projectRepositoryMock.Object,
                jwtLibraryWrapperMock.Object, _logger, null, null, null, null);

            // act
            string jsonResult = tokenService.VerifyToken(_jwtValid, _sampleGuid);
            var deserialized = JsonConvert.DeserializeObject<JsonObject>(jsonResult);

            // assert
            Assert.Equal(JsonValues.TokenValid, deserialized.Result);
            Assert.Null(deserialized.Jwt);
            Assert.Null(deserialized.ErrorDetails);
        }
    }
}