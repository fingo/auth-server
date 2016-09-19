using System.Collections.Generic;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IGetAllFromProjectUser
    {
        IEnumerable<UserModel> Invoke(int projectId);
    }
}