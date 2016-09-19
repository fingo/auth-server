namespace Fingo.Auth.Domain.CustomData.ConfigurationClasses
{
    public abstract class UserConfigurationView
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string ConfigurationName { get; set; }
    }
}