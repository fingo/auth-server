using Fingo.Auth.Domain.Infrastructure.Interfaces;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Interfaces
{
    public interface IGetPoliciesFromProjectFactory : IActionFactory
    {
        IGetPoliciesFromProject Create();
    }
}
