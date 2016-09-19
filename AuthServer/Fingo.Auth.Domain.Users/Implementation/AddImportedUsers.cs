using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.User;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class AddImportedUsers : IAddImportedUsers
    {
        private readonly IUserRepository repository;
        private readonly IProjectRepository projectRepository;
        private readonly IEventBus eventBus;
        private readonly ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject;

        public AddImportedUsers(IUserRepository repository, IProjectRepository projectRepository,
            IEventBus eventBus, ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
            this.setDefaultUserCustomDataBasedOnProject = setDefaultUserCustomDataBasedOnProject;
            this.eventBus = eventBus;
        }

        public void Invoke(List<UserModel> userModels, int projectId,
            ref int usersAdded, ref int userdDuplicated, ref List<Tuple<string,string>> userAddedEmails)
        {
            var project = projectRepository.GetById(projectId).WithoutStatuses(ProjectStatus.Deleted);

            if (project == null)
                throw new ArgumentNullException($"Cannot add user to project with id: {projectId}.");

            var attribute= new EmailAddressAttribute();

            foreach (var userModel in userModels)
            {
                if (!attribute.IsValid(userModel.Login))
                    continue;

                var newUser = repository.GetAll().FirstOrDefault(x => x.Login == userModel.Login);

                if (newUser == null)
                {
                    var user = new User
                    {
                        PasswordSalt = string.Empty,
                        ActivationToken = Guid.NewGuid().ToString().Replace("-", string.Empty),
                        Login = userModel.Login,
                        Password = string.Empty,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Status = UserStatus.AccountCreated,
                        ProjectUsers = new List<ProjectUser>(),
                        LastPasswordChange = default(DateTime)
                    };

                    userAddedEmails.Add(new Tuple<string, string>(user.Login, user.ActivationToken));
                    user.ProjectUsers.Add(new ProjectUser { ProjectId = project.Id });

                    setDefaultUserCustomDataBasedOnProject.SetDefaultUserCustomData(project, user);

                    usersAdded++;

                    repository.Add(user);
                }
                else
                {
                    if (newUser.Status != UserStatus.Deleted)
                    {
                        userdDuplicated++;
                        eventBus.Publish(new DuplicateUserImported(newUser.FirstName, newUser.LastName, newUser.Login, projectId));
                        continue;
                    }

                    newUser.PasswordSalt = string.Empty;
                    newUser.FirstName = userModel.FirstName;
                    newUser.LastName = userModel.LastName;
                    newUser.Password = string.Empty;
                    newUser.ActivationToken = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    newUser.Status = UserStatus.AccountCreated;
                    newUser.LastPasswordChange = default(DateTime);
                    newUser.ProjectUsers = new List<ProjectUser> { new ProjectUser { ProjectId = project.Id } };

                    setDefaultUserCustomDataBasedOnProject.SetDefaultUserCustomData(project, newUser);

                    userAddedEmails.Add(new Tuple<string, string>(newUser.Login, newUser.ActivationToken));
                    usersAdded++;
                    repository.Edit(newUser);
                }
            }
            eventBus.Publish(new UsersImported(usersAdded, userdDuplicated, userModels.Count, projectId));
        }
    }
}