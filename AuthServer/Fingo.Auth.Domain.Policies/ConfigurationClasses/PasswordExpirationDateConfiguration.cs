using Fingo.Auth.Domain.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.ConfigurationClasses
{
    public class PasswordExpirationDateConfiguration : PolicyConfiguration
    {
        public PasswordExpiration PasswordExpiration { get; set; }
        public int? CustomValue { get; set; }
    }
}