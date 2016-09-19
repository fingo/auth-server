using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IGetUserByActivationToken
    {
        UserModel Invoke(string activationToken);
    }
}
