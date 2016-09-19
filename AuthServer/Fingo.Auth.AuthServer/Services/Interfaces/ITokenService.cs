using System;

namespace Fingo.Auth.AuthServer.Services.Interfaces
{
    public interface ITokenService
    {
        string AcquireToken(string login, string password, Guid projectGuid);
        string VerifyToken(string jwtToValidate, Guid projectGuid);
    }
}