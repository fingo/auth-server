using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Infrastructure.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Interfaces
{
    public interface IGetAllCustomDataFromProjectFactory : IActionFactory
    {
        IGetAllCustomDataFromProject Create();
    }
}