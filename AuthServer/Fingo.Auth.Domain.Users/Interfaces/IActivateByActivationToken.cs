namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IActivateByActivationToken
    {
        void Invoke(string activationToken);
    }
}
