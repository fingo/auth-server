using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Services.Implementation;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.Services.Implementation
{
    public class JsonConvertServiceTest
    {
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