namespace Fingo.Auth.AuthServer.Client.Services.Interfaces
{
    public interface IRemoteTokenService
    {
        bool VerifyToken(string jwt);
        string AcquireToken(string login , string password);
    }
}