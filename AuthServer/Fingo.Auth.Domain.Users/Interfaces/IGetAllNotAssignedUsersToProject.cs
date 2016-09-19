using System.Collections.Generic;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IGetAllNotAssignedUsersToProject
    {
        IEnumerable<BaseUserModel> Invoke(int projectId);
    }
}