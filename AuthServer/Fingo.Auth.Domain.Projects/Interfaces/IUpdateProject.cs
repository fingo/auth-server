namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IUpdateProject
    {
        void Invoke(int id, string newName);
    }
}