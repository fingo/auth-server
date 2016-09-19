using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IGetUserByLogin
    {
        UserModel Invoke(string login);
    }
}