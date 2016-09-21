using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.User;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class AddUser : IAddUser
    {
        private readonly IEventBus _eventBus;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _repository;
        private readonly ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject;

        public AddUser(IUserRepository repository , IProjectRepository projectRepository , IEventBus eventBus ,
            ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            this.setDefaultUserCustomDataBasedOnProject = setDefaultUserCustomDataBasedOnProject;
            _eventBus = eventBus;
        }

        public void Invoke(UserModel userModel , Guid guid)
        {
            var project = _projectRepository.GetByGuid(guid).WithoutStatuses(ProjectStatus.Deleted);

            if (project == null)
                throw new ArgumentNullException(
                    $"Cannot add user to project with Guid:{guid}, because this project has been deleted.");

            var newUser = _repository.GetAll().FirstOrDefault(x => x.Login == userModel.Login);

            if (newUser == null)
            {
                var user = new User
                {
                    PasswordSalt = userModel.PasswordSalt ,
                    ActivationToken = userModel.ActivationToken ,
                    Login = userModel.Login ,
                    Password = userModel.Password ,
                    FirstName = userModel.FirstName ,
                    LastName = userModel.LastName ,
                    ProjectUsers = new List<ProjectUser>() ,
                    LastPasswordChange = DateTime.UtcNow
                };

                user.ProjectUsers.Add(new ProjectUser {ProjectId = project.Id});

                setDefaultUserCustomDataBasedOnProject.SetDefaultUserCustomData(project , user);

                _repository.Add(user);

                _eventBus.Publish(new UserAdded(user.FirstName , user.LastName , user.Login));
                return;
            }

            newUser.PasswordSalt = userModel.PasswordSalt;
            newUser.FirstName = userModel.FirstName;
            newUser.LastName = userModel.LastName;
            newUser.Password = userModel.Password;
            newUser.ActivationToken = userModel.ActivationToken;
            newUser.Status = UserStatus.Registered;
            newUser.LastPasswordChange = DateTime.UtcNow;
            newUser.ProjectUsers = new List<ProjectUser> {new ProjectUser {ProjectId = project.Id}};

            setDefaultUserCustomDataBasedOnProject.SetDefaultUserCustomData(project , newUser);

            _repository.Edit(newUser);
            _eventBus.Publish(new UserMerged(newUser.Login , newUser.FirstName , newUser.LastName));
        }
    }
}