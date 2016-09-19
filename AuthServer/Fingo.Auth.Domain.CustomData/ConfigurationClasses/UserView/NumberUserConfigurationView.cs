namespace Fingo.Auth.Domain.CustomData.ConfigurationClasses.UserView
{
    public class NumberUserConfigurationView : UserConfigurationView
    {
        public int CurrentValue { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
    }
}