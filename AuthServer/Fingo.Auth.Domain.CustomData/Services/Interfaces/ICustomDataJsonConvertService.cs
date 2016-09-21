using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses;

namespace Fingo.Auth.Domain.CustomData.Services.Interfaces
{
    public interface ICustomDataJsonConvertService
    {
        string Serialize(CustomDataConfiguration configuration);
        UserConfiguration DeserializeUser(ConfigurationType type , string serialized);
        ProjectConfiguration DeserializeProject(ConfigurationType type , string serialized);
    }
}