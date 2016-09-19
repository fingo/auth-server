using System;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Services.Interfaces;
using Newtonsoft.Json;

namespace Fingo.Auth.Domain.Policies.Services.Implementation
{
    public class PolicyJsonConvertService : IPolicyJsonConvertService
    {
        public string Serialize(PolicyConfiguration policyConfiguration)
        {
            return JsonConvert.SerializeObject(policyConfiguration);
        }

        public PolicyConfiguration Deserialize(Policy policy, string serialized)
        {
            switch (policy)
            {
                case Policy.AccountExpirationDate:
                    return JsonConvert.DeserializeObject<AccountExpirationDateConfiguration>(serialized);
                case Policy.MinimumPasswordLength:
                    return JsonConvert.DeserializeObject<MinimumPasswordLengthConfiguration>(serialized);
                case Policy.PasswordExpirationDate:
                    return JsonConvert.DeserializeObject<PasswordExpirationDateConfiguration>(serialized);
                case Policy.RequiredPasswordCharacters:
                    return JsonConvert.DeserializeObject<RequiredPasswordCharactersConfiguration>(serialized);
                case Policy.ExcludeCommonPasswords:
                    return JsonConvert.DeserializeObject<ExcludeCommonPasswordsConfiguration>(serialized);
                default:
                    throw new ArgumentException("Wrong policy given to Deserialize()");
            }
        }
        public PolicyConfiguration DeserializeUser(Policy policy, string serialized)
        {
            switch (policy)
            {
                case Policy.AccountExpirationDate:
                    return JsonConvert.DeserializeObject<UserAccountExpirationDateConfiguration>(serialized);
                default:
                    throw new ArgumentException("Wrong policy given to Deserialize()");
            }
        }
    }
}
