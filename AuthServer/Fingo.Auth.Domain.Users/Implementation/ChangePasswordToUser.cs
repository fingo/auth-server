using System;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.User;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class ChangePasswordToUser : IChangePasswordToUser
    {
        private IUserRepository _userRepository;
        private readonly IEventBus _eventBus;

        public ChangePasswordToUser(IUserRepository userRepository, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventBus = eventBus;
        }

        public void Invoke(ChangingPasswordUser userWithNewPassword)
        {
            User user = _userRepository.GetAll().FirstOrDefault(u => u.Login == userWithNewPassword.Email);

            if (user == null)
                throw new ArgumentNullException($"Cannot change password for user with email: {userWithNewPassword.Email}, because this user does not exist.");

            if (user.Password != userWithNewPassword.Password)
            {
                throw new Exception($"Cannot change password for user with email: {userWithNewPassword.Email}, because password was wrong.");
            }

            user.Password = userWithNewPassword.NewPassword;
            user.LastPasswordChange = DateTime.UtcNow;

            _userRepository.Edit(user);

            _eventBus.Publish(new UserChangedPassword(user.Id, user.FirstName, user.LastName));
        }

       
    }
}