using Fingo.Auth.DbAccess.Models;

namespace Fingo.Auth.Domain.Users.Services.Interfaces
{
    public interface ISetDefaultUserCustomDataBasedOnProject
    {
        void SetDefaultUserCustomData(Project project , User newUser);
    }
}