using System;
using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models;

namespace Fingo.Auth.Domain.Models.UserModels
{
    public class UserModel : BaseUserModel
    {
        public UserModel()
        {
        }

        public UserModel(User user) : base(user)
        {
            ModificationDate = user.ModificationDate;
            Password = user.Password;
            CreationDate = user.CreationDate;
        }

        public DateTime CreationDate { get; private set; }

        public DateTime ModificationDate { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
    }
}