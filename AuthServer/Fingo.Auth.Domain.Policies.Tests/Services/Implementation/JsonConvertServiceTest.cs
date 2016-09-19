using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Services.Implementation;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.Services.Implementation
{
    public class JsonConvertServiceTest
    {
        [Fact]
        public void AccountExpirationDateConfigurationShouldSerializeAndDeserialize()
        {
            // arrange
            var dictionary = new Dictionary<int, DateTime>
            {
                {23, DateTime.UtcNow},
                {17, DateTime.MaxValue},
                {1, DateTime.Now},
                {42, DateTime.MinValue}
            };
            var isEnabled = false;

            var configuration = new AccountExpirationDateConfiguration
            {
                //TODO:repair
                //UserIdDateDictionary = dictionary,
                IsEnabled = isEnabled
            };
            var service = new PolicyJsonConvertService();

            // act
            var serialized = service.Serialize(configuration);
            var deserialized = (AccountExpirationDateConfiguration) service.Deserialize(Policy.AccountExpirationDate, serialized);

            // assert
            //TODO:repair
            //Assert.Equal(dictionary, deserialized.UserIdDateDictionary);
            Assert.Equal(isEnabled, deserialized.IsEnabled);
        }

        [Fact]
        public void MinimumPasswordLengthConfigurationShouldSerializeAndDeserialize()
        {
            // arrange
            var length = 42;
            var configuration = new MinimumPasswordLengthConfiguration
            {
                Length = length
            };
            var service = new PolicyJsonConvertService();

            // act
            var serialized = service.Serialize(configuration);
            var deserialized = (MinimumPasswordLengthConfiguration)service.Deserialize(Policy.MinimumPasswordLength, serialized);

            // assert
            Assert.Equal(length, deserialized.Length);
        }

        [Fact]
        public void PasswordExpirationDateConfigurationShouldSerializeAndDeserialize()
        {
            // arrange
            var date = DateTime.UtcNow;
            var configuration = new PasswordExpirationDateConfiguration
            {
            };
            var service = new PolicyJsonConvertService();

            // act
            var serialized = service.Serialize(configuration);
            var deserialized = (PasswordExpirationDateConfiguration)service.Deserialize(Policy.PasswordExpirationDate, serialized);

            // assert
            //Assert.Equal(date, deserialized.Date);
        }

        [Fact]
        public void RequiredPasswordCharactersConfigurationShouldSerializeAndDeserialize()
        {
            // arrange
            var upperCase = false;
            var digit = true;
            var special = true;
            var configuration = new RequiredPasswordCharactersConfiguration
            {
                UpperCase = upperCase,
                Digit = digit,
                Special = special
            };
            var service = new PolicyJsonConvertService();

            // act
            var serialized = service.Serialize(configuration);
            var deserialized = (RequiredPasswordCharactersConfiguration)service.Deserialize(Policy.RequiredPasswordCharacters, serialized);

            // assert
            Assert.Equal(digit, deserialized.Digit);
            Assert.Equal(special, deserialized.Special);
            Assert.Equal(upperCase, deserialized.UpperCase);
        }
    }
}
