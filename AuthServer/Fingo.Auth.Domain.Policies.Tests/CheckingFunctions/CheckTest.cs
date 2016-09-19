using System;
using Fingo.Auth.Domain.Policies.CheckingFunctions;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Enums;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.CheckingFunctions
{
    public class CheckTest
    {
        [Fact]
        public void AccountExpirationDateCheckShouldReturnTrueBecauseOfNotEnabled()
        {
            // arrange
            var configuration = new AccountExpirationDateConfiguration{IsEnabled = false};
            var userConfiguration = new UserAccountExpirationDateConfiguration() { ExpirationDate = DateTime.Now.AddDays(1) };

            // act
            var checkResult = Check.AccountExpirationDate(configuration, userConfiguration);

            // assert
            Assert.True(checkResult);
        }

        [Fact]
        public void AccountExpirationDateCheckShouldReturnTrue()
        {
            // arrange
            var configuration = new AccountExpirationDateConfiguration { IsEnabled = true };
            var userConfiguration = new UserAccountExpirationDateConfiguration() { ExpirationDate = DateTime.Now.AddDays(1) };

            // act
            var checkResult = Check.AccountExpirationDate(configuration, userConfiguration);

            // assert
            Assert.True(checkResult);
        }

        [Fact]
        public void AccountExpirationDateCheckShouldReturnTrueBecauseUserAccountExpirationDateIsNotSet()
        {
            // arrange
            var configuration = new AccountExpirationDateConfiguration { IsEnabled = true };
            var userConfiguration = new UserAccountExpirationDateConfiguration() { ExpirationDate = default(DateTime) };

            // act
            var checkResult = Check.AccountExpirationDate(configuration, userConfiguration);

            // assert
            Assert.True(checkResult);
        }

        [Fact]
        public void AccountExpirationDateCheckShouldReturnFalseBecauseOfDateExpired()
        {
            // arrange
            var configuration = new AccountExpirationDateConfiguration{ IsEnabled = true };
            var userConfiguration=new UserAccountExpirationDateConfiguration() {ExpirationDate = DateTime.Now.AddDays(-1)};

            // act

            var checkResult = Check.AccountExpirationDate(configuration, userConfiguration);

            // assert
            Assert.False(checkResult);
        }

        [Fact]
        public void MinimumPasswordLengthCheckShouldReturnTrue()
        {
            // arrange
            var configuration = new MinimumPasswordLengthConfiguration
            {
                Length = 7
            };

            // act
            var checkResult = Check.MinimumPasswordLength(configuration, "password");

            // assert
            Assert.True(checkResult);
        }

        [Fact]
        public void MinimumPasswordLengthCheckShouldReturnFalse()
        {
            // arrange
            var configuration = new MinimumPasswordLengthConfiguration
            {
                Length = 10
            };

            // act
            var checkResult = Check.MinimumPasswordLength(configuration, "password");

            // assert
            Assert.False(checkResult);
        }

        [Fact]
        public void PasswordExpirationDateCheckShouldReturnTrue()
        {
            // arrange
            var configuration = new PasswordExpirationDateConfiguration
            {
                PasswordExpiration = PasswordExpiration.Custom,
                CustomValue = 42
            };

            // act
            var checkResult = Check.PasswordExpirationDate(configuration, DateTime.UtcNow.AddDays(-41));

            // assert
            Assert.True(checkResult);
        }

        [Fact]
        public void PasswordExpirationDateCheckShouldReturnFalseBecauseOfDateExpired()
        {
            // arrange
            var configuration = new PasswordExpirationDateConfiguration
            {
                PasswordExpiration = PasswordExpiration.Month
            };

            // act
            var checkResult = Check.PasswordExpirationDate(configuration, DateTime.Now.AddMonths(-2));

            // assert
            Assert.False(checkResult);
        }

        [Fact]
        public void RequiredPasswordCharactersCheckShouldReturnTrue()
        {
            // arrange
            var configuration1 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = false,
                Digit = false,
                Special = true,
            };

            var configuration2 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = true,
                Digit = false,
                Special = false,
            };

            var configuration3 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = false,
                Digit = true,
                Special = false,
            };

            var configuration4 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = true,
                Digit = true,
                Special = true,
            };

            // act
            var checkResult1 = Check.RequiredPasswordCharacters(configuration1, "hello!");
            var checkResult2 = Check.RequiredPasswordCharacters(configuration2, "Hello");
            var checkResult3 = Check.RequiredPasswordCharacters(configuration3, "h3llo");
            var checkResult4 = Check.RequiredPasswordCharacters(configuration4, "H3llo!");

            // assert
            Assert.True(checkResult1);
            Assert.True(checkResult2);
            Assert.True(checkResult3);
            Assert.True(checkResult4);
        }

        [Fact]
        public void RequiredPasswordCharactersCheckShouldReturnFalse()
        {
            // arrange
            var configuration1 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = false,
                Digit = false,
                Special = true,
            };

            var configuration2 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = true,
                Digit = false,
                Special = false,
            };

            var configuration3 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = false,
                Digit = true,
                Special = false,
            };

            var configuration4 = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = true,
                Digit = true,
                Special = true,
            };

            // act
            var checkResult1 = Check.RequiredPasswordCharacters(configuration1, "H3llo");
            var checkResult2 = Check.RequiredPasswordCharacters(configuration2, "h3llo!");
            var checkResult3 = Check.RequiredPasswordCharacters(configuration3, "Hello!");
            var checkResult4 = Check.RequiredPasswordCharacters(configuration4, "hello");

            // assert
            Assert.False(checkResult1);
            Assert.False(checkResult2);
            Assert.False(checkResult3);
            Assert.False(checkResult4);
        }
    }
}
