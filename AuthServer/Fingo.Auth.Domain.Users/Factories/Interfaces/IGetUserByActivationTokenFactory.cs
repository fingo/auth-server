using Fingo.Auth.Domain.Infrastructure.Interfaces;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Factories.Interfaces
{
    public interface IGetUserByActivationTokenFactory : IActionFactory
    {
        IGetUserByActivationToken Create();
    }
}