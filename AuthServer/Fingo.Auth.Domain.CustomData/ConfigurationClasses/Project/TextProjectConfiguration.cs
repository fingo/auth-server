using System.Collections.Generic;

namespace Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project
{
    public class TextProjectConfiguration : ProjectConfiguration
    {
        public TextProjectConfiguration()
        {
            PossibleValues = new HashSet<string>();
        }

        public string Default { get; set; }
        public HashSet<string> PossibleValues { get; set; }
    }
}