using System;

namespace Fingo.Auth.Domain.Policies.ConfigurationClasses
{
    public class UserAccountExpirationDateConfiguration:PolicyConfiguration
    {
        public DateTime ExpirationDate { get; set; }
    }
}