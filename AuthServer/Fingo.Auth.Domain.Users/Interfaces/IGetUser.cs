using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IGetUser
    {
        BaseUserModelWithProjects Invoke(int id);
    }
}