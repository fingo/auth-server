using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.User;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Users.Interfaces;
using Fingo.Auth.Domain.Users.Services.Interfaces;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class AssignUser: IAssignUser
    {
        private readonly IProjectRepository projectRepository;
        private readonly IEventBus eventBus;
        private readonly IUserRepository userRepository;
        private readonly ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject;

        public AssignUser(IProjectRepository projectRepository , IEventBus eventBus, IUserRepository userRepository,ISetDefaultUserCustomDataBasedOnProject setDefaultUserCustomDataBasedOnProject)
        {
            this.projectRepository = projectRepository;
            this.eventBus = eventBus;
            this.userRepository = userRepository;
            this.setDefaultUserCustomDataBasedOnProject = setDefaultUserCustomDataBasedOnProject;
        }

        public void Invoke(int projectId , int userId)
        {
            Project project = projectRepository.GetById(projectId).WithoutStatuses(ProjectStatus.Deleted);

            if (project == null)
                throw new ArgumentNullException(
                    $"Cannot assign users to project with id:{projectId}, because this project non exist.");

            if (project.ProjectUsers == null)
            {
                project.ProjectUsers = new List<ProjectUser>();
            }

            User user = userRepository.GetById(userId).WithoutStatuses(UserStatus.Deleted);

            if (user == null)
                throw new ArgumentNullException(
                    $"Cannot assign user to project with id:{projectId}, because user non exist.");

            project.ProjectUsers.Add(new ProjectUser() {UserId = user.Id});

            setDefaultUserCustomDataBasedOnProject.SetDefaultUserCustomData(project , user);

            userRepository.Edit(user);
            projectRepository.Edit(project);
            eventBus.Publish(new UserAssigned(project.Id , project.Name , userId));
        }
    }
}