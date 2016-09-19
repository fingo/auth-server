using System.ComponentModel.DataAnnotations;

namespace Fingo.Auth.Domain.Models.UserModels
{
    public class ChangingPasswordUser
    {
        public string Email { get; set; }

        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
