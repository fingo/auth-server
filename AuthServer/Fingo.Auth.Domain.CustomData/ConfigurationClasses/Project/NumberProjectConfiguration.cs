namespace Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project
{
    public class NumberProjectConfiguration : ProjectConfiguration
    {
        public int Default { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
    }
}
