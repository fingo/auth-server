namespace Fingo.Auth.ManagementApp.Services.Interfaces
{
    public interface ITokenService
    {
        string RegistrationState(string jwt);
    }
}