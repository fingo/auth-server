using System;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.AuthServer.Services.Interfaces
{
    public interface IAccountService
    {
        string CreateNewUserInProject(UserModel user , Guid projectGuid);
        string PasswordChangeForUser(ChangingPasswordUser user);
        string SetPasswordForUser(string token , string password);
    }
}