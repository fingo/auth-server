namespace Fingo.Auth.AuthServer.Services.Interfaces
{
    public interface IHashingService
    {
        string HashWithDatabaseSalt(string password , string userLogin);
        string HashWithGivenSalt(string password , string hexSalt);
        string GenerateNewSalt();
    }
}