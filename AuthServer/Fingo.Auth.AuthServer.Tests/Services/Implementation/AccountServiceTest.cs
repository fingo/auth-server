using System;
using Fingo.Auth.AuthServer.Services.Implementation;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Factories.Interfaces;
using Fingo.Auth.Infrastructure.Logging;
using Fingo.Auth.JsonWrapper;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Fingo.Auth.AuthServer.Tests.Services.Implementation
{
    public class AccountServiceTest
    {
        private readonly ILogger<AccountService> _logger;
        public AccountServiceTest()
        {
            _logger = new Logger<AccountService>();
        }

        //TO REPAIR!!!
        [Fact]
        void CreateNewUserInProjectShouldReturnErrorBecauseOfFactoryException()
        {
            // arrange

            Mock<IChangePasswordToUserFactory> fac=new Mock<IChangePasswordToUserFactory>();
            var addUserFactoryMock = new Mock<IAddUserFactory>();

            addUserFactoryMock.Setup(fact => fact.Create().Invoke(It.IsAny<UserModel>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            IAccountService accountService = new AccountService(addUserFactoryMock.Object, _logger, fac.Object, null, null, null, null);

            // act
            string jsonResult = accountService.CreateNewUserInProject(new UserModel(), Guid.NewGuid());
            var deserialized = JsonConvert.DeserializeObject<JsonObject>(jsonResult);

            // assert
            Assert.Equal(JsonValues.Error, deserialized.Result);
            Assert.NotNull(deserialized.ErrorDetails);
        }
    }
}
