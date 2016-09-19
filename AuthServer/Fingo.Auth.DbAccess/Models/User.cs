using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.Policies;

namespace Fingo.Auth.DbAccess.Models
{
    public class User : BaseEntityWithUserStatus
    {
        public User()
        {
            LastPasswordChange = CreationDate;
            UserCustomData=new List<UserCustomData>();
            UserPolicies=new List<UserPolicies>();
        }

        [MaxLength(30, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

        [MaxLength(30, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime LastPasswordChange { get; set; }

        public string ActivationToken { get; set; }

        public string PasswordSalt { get; set; }

        public virtual List<ProjectUser> ProjectUsers { get; set; }

        public virtual List<UserCustomData> UserCustomData { get; set; }
        public virtual List<UserPolicies> UserPolicies { get; set; }
    }
}