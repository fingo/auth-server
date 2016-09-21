using Fingo.Auth.Domain.Infrastructure.Interfaces;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Factories.Interfaces
{
    public interface IGetAddressForSetPasswordFactory : IActionFactory
    {
        IGetAddressForSetPassword Create();
    }
}