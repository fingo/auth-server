using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.AuthServer.Client.Services.Interfaces
{
    public interface IRemoteAccountService
    {
        void CreateNewUserInProject(UserModel user);
        void PasswordChangeForUser(ChangingPasswordUser user);
        void SetPasswordForUser(string token , string password);
    }
}