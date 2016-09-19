using System.Collections.Generic;

namespace Fingo.Auth.Domain.CustomData.ConfigurationClasses.UserView
{
    public class TextUserConfigurationView:UserConfigurationView
    {
        public string CurrentValue { get; set; }
        public List<string> PossibleValuesList { get; set; }
    }
}