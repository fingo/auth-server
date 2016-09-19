using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;

namespace Fingo.Auth.Domain.Policies.Services.Interfaces
{
    public interface IPolicyJsonConvertService
    {
        string Serialize(PolicyConfiguration policyConfiguration);
        PolicyConfiguration Deserialize(Policy policy, string serialized);
        PolicyConfiguration DeserializeUser(Policy policy , string serialized);
    }
}