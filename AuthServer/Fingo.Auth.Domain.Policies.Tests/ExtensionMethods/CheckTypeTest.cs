using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.Enums;
using Fingo.Auth.Domain.Policies.ExtensionMethods;
using Xunit;

namespace Fingo.Auth.Domain.Policies.Tests.ExtensionMethods
{
    public class CheckTypeTest
    {
        [Fact]
        public void WithTypesShouldReturnNull()
        {
            // arrange
            List<Policy> list = null;

            // act & assert
            Assert.Equal(null, list.WithTypes(PolicyType.AccountCreation));
        }

        [Fact]
        public void WithTypesShouldReturnLogInTypes()
        {
            // arrange
            var list = new List<Policy>
            {
                Policy.PasswordExpirationDate,
                Policy.MinimumPasswordLength,
                Policy.RequiredPasswordCharacters,
                Policy.AccountExpirationDate
            };

            // act & assert
            Assert.Equal(
                new List<Policy>
                {
                    Policy.PasswordExpirationDate,
                    Policy.AccountExpirationDate
                },
                list.WithTypes(PolicyType.LogIn));
        }

        [Fact]
        public void WithTypesShouldReturnAccountCreationTypes()
        {
            // arrange
            var list = new List<Policy>
            {
                Policy.PasswordExpirationDate,
                Policy.MinimumPasswordLength,
                Policy.RequiredPasswordCharacters,
                Policy.AccountExpirationDate
            };

            // act & assert
            Assert.Equal(new List<Policy> { Policy.MinimumPasswordLength, Policy.RequiredPasswordCharacters }, list.WithTypes(PolicyType.AccountCreation));
        }

        [Fact]
        public void WithTypesShouldReturnWhatWasGiven()
        {
            // arrange
            var list = new List<Policy>
            {
                Policy.RequiredPasswordCharacters,
                Policy.AccountExpirationDate
            };

            // act & assert
            Assert.Equal(list, list.WithTypes());
        }

        [Fact]
        public void WithTypesTupleShouldReturnNull()
        {
            // arrange
            List<Tuple<Policy, object>> list = null;

            // act & assert
            Assert.Equal(null, list.WithTypes(PolicyType.AccountCreation));
        }

        [Fact]
        public void WithTypesTupleShouldReturnLogInTypes()
        {
            // arrange
            var list = new List<Tuple<Policy, int>>
            {
                new Tuple<Policy, int>(Policy.PasswordExpirationDate, 2),
                new Tuple<Policy, int>(Policy.MinimumPasswordLength, 42),
                new Tuple<Policy, int>(Policy.RequiredPasswordCharacters, -20000000),
                new Tuple<Policy, int>(Policy.AccountExpirationDate, -41)
            };

            // act & assert
            Assert.Equal(
                new List<Tuple<Policy, int>>
                {
                    new Tuple<Policy, int>(Policy.PasswordExpirationDate, 2),
                    new Tuple<Policy, int>(Policy.AccountExpirationDate, -41)
                },
                list.WithTypes(PolicyType.LogIn));
        }

        [Fact]
        public void WithTypesTupleShouldReturnAccountCreationTypes()
        {
            // arrange
            var list = new List<Tuple<Policy, double>>
            {
                new Tuple<Policy, double>(Policy.PasswordExpirationDate, Math.E),
                new Tuple<Policy, double>(Policy.MinimumPasswordLength, Math.PI),
                new Tuple<Policy, double>(Policy.RequiredPasswordCharacters, Math.Exp(Math.PI)),
                new Tuple<Policy, double>(Policy.AccountExpirationDate, 42)
            };

            // act & assert
            Assert.Equal(
                new List<Tuple<Policy, double>>
                {
                    new Tuple<Policy, double>(Policy.MinimumPasswordLength, Math.PI),
                    new Tuple<Policy, double>(Policy.RequiredPasswordCharacters, Math.Exp(Math.PI))
                },
                list.WithTypes(PolicyType.AccountCreation));
        }

        [Fact]
        public void WithTypesTupleShouldReturnWhatWasGiven()
        {
            // arrange
            var list = new List<Tuple<Policy, string>>
            {
                new Tuple<Policy, string>(Policy.RequiredPasswordCharacters, "a"),
                new Tuple<Policy, string>(Policy.AccountExpirationDate, "b")
            };

            // act & assert
            Assert.Equal(list, list.WithTypes());
        }
    }
}