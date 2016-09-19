namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IAssignUser
    {
        void Invoke(int projectId , int usersId);
    }
}