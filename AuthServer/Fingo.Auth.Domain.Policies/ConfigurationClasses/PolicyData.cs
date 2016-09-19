using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.ConfigurationClasses
{
    public static class PolicyData
    {
        public static Dictionary<Policy, string> Name = new Dictionary<Policy, string>
        {
            {Policy.PasswordExpirationDate, "Password expiration date" },
            {Policy.AccountExpirationDate, "Account expiration date" },
            {Policy.MinimumPasswordLength, "Minimum password length" },
            {Policy.RequiredPasswordCharacters, "Required password characters" },
            {Policy.ExcludeCommonPasswords, "Exclude common passwords" }
        };

        public static Dictionary<Policy, PolicyType> Type = new Dictionary<Policy, PolicyType>
        {
            {Policy.PasswordExpirationDate, PolicyType.LogIn },
            {Policy.AccountExpirationDate, PolicyType.LogIn },
            {Policy.MinimumPasswordLength, PolicyType.AccountCreation },
            {Policy.RequiredPasswordCharacters, PolicyType.AccountCreation },
            {Policy.ExcludeCommonPasswords, PolicyType.AccountCreation }
        };

        public static Dictionary<Policy, bool> IsConfigurablePerUser = new Dictionary<Policy, bool>
        {
            {Policy.PasswordExpirationDate, false },
            {Policy.AccountExpirationDate, true },
            {Policy.MinimumPasswordLength, false },
            {Policy.RequiredPasswordCharacters, false },
            {Policy.ExcludeCommonPasswords, false }
        };
    }
}