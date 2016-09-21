namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IGetAddressForSetPassword
    {
        string Invoke(int projectId);
    }
}