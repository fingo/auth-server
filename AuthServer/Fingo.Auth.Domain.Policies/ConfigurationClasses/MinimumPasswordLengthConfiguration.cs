namespace Fingo.Auth.Domain.Policies.ConfigurationClasses
{
    public class MinimumPasswordLengthConfiguration : PolicyConfiguration
    {
        public int Length { get; set; }
    }
}