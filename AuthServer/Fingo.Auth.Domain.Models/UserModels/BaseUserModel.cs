using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;

namespace Fingo.Auth.Domain.Models.UserModels
{
    public class BaseUserModel
    {
        public BaseUserModel()
        {

        }

        public BaseUserModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Login = user.Login;
            UserStatus = user.Status;
            ActivationToken = user.ActivationToken;
            PasswordSalt = user.PasswordSalt;
        }
        public int Id { get; set; }

        [MaxLength(30, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

        [MaxLength(30, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }

        public string PasswordSalt { get; set; }

        
        [Required(ErrorMessage = "The email address is required")]      
        public string Login { get; set; }

        public UserStatus UserStatus {get; set;}

        public string ActivationToken { get; set; }
    }
}