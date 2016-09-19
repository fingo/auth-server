using System;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.Project;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;
using Newtonsoft.Json;

namespace Fingo.Auth.Domain.CustomData.Services.Implementation
{
    public class CustomDataJsonConvertService : ICustomDataJsonConvertService
    {
        public string Serialize(CustomDataConfiguration configuration)
        {
            return JsonConvert.SerializeObject(configuration);
        }

        public UserConfiguration DeserializeUser(ConfigurationType type, string serialized)
        {
            switch (type)
            {
                case ConfigurationType.Number:
                    return JsonConvert.DeserializeObject<NumberUserConfiguration>(serialized);
                case ConfigurationType.Boolean:
                    return JsonConvert.DeserializeObject<BooleanUserConfiguration>(serialized);
                case ConfigurationType.Text:
                    return JsonConvert.DeserializeObject<TextUserConfiguration>(serialized);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid ConfigurationType.");
            }
        }

        public ProjectConfiguration DeserializeProject(ConfigurationType type, string serialized)
        {
            switch (type)
            {
                case ConfigurationType.Number:
                    return JsonConvert.DeserializeObject<NumberProjectConfiguration>(serialized);
                case ConfigurationType.Boolean:
                    return JsonConvert.DeserializeObject<BooleanProjectConfiguration>(serialized);
                case ConfigurationType.Text:
                    return JsonConvert.DeserializeObject<TextProjectConfiguration>(serialized);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid ConfigurationType.");
            }
        }
    }
}
