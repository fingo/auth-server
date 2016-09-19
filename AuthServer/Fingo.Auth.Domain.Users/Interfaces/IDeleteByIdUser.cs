namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IDeleteByIdUser
    {
        void Invoke(int projectId , int userId);
    }
}