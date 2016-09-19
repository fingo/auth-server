using System.ComponentModel.DataAnnotations;

namespace Fingo.Auth.Domain.Models.UserModels
{
    public class SettingPasswordUser
    {
        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}