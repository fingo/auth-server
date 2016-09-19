namespace Fingo.Auth.Domain.Policies.ConfigurationClasses
{
    public class RequiredPasswordCharactersConfiguration : PolicyConfiguration
    {
        public bool Digit { get; set; }
        public bool UpperCase { get; set; }
        public bool Special { get; set; }
    }
}