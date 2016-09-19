using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IChangePasswordToUser
    {
        void Invoke(ChangingPasswordUser user);
    }
}