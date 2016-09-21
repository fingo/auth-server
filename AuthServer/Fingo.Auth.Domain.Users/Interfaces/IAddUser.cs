using System;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Users.Interfaces
{
    public interface IAddUser
    {
        void Invoke(UserModel user , Guid guid);
    }
}