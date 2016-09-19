using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.Statuses;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class DeleteProject : IDeleteProject
    {
        private readonly IProjectRepository _repo;
        private readonly IEventBus _eventBus;

        public DeleteProject(IProjectRepository repo, IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }
        public void Invoke(int id)
        {
            Project toRemove = _repo.GetByIdWithAll(id).WithoutStatuses(ProjectStatus.Deleted);
            if (toRemove == null)
            {
                throw new ArgumentNullException($"Cannot find project with id={id}.");
            }
            toRemove.ModificationDate = DateTime.UtcNow;
            toRemove.Status = ProjectStatus.Deleted;
            toRemove.ProjectCustomData = null;
            toRemove.ProjectPolicies = null;
            toRemove.ProjectUsers = null;
            _repo.Edit(toRemove);

            _eventBus.Publish(new ProjectRemoved(toRemove.Id, toRemove.Name));
        }
    }
}