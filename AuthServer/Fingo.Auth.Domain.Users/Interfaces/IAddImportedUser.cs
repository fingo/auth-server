using System;
using System.Collections.Generic;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IAddImportedUsers
    {
        void Invoke(List<UserModel> userModels , int projectId, ref int usersAdded,
            ref int userdDuplicated, ref List<Tuple<string, string>> userAddedEmails);
    }
}