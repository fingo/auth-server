using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Users.Interfaces;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.User;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;

namespace Fingo.Auth.Domain.Users.Implementation
{
    public class DeleteByIdUser : IDeleteByIdUser
    {
        private readonly IUserRepository _repository;
        private readonly IEventBus _eventBus;

        public DeleteByIdUser(IUserRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public void Invoke(int projectId, int userId)
        {
            User user = _repository.GetById(userId).WithoutStatuses(UserStatus.Deleted);
            if (user == null)
            {
                throw new ArgumentNullException($"Cannot find user with id={userId}");
            }

            var toRemove = user.ProjectUsers.FirstOrDefault(pu => pu.ProjectId == projectId);
            var removeUserCustomDataFromProject =
                user.UserCustomData.Where(m => m.UserId == userId && m.ProjectCustomData.ProjectId == projectId);
            var removeUserPolicy =
                user.UserPolicies.Where(m=>m.ProjectPolicies.ProjectId == projectId);

            if (toRemove == null)
            {
                throw new ArgumentNullException($"Cannot find project with id={projectId} connected with user id={userId}");
            }

            user.ProjectUsers.Remove(toRemove);

            user.UserCustomData = user.UserCustomData.Except(removeUserCustomDataFromProject).ToList();
            user.UserPolicies = user.UserPolicies.Except(removeUserPolicy).ToList();

            _repository.Edit(user);

            _eventBus.Publish(new UserRemovedFromProject(user.FirstName , user.LastName , user.Login , toRemove.ProjectId));
        }
    }
}