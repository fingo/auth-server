using System;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.ProjectModels;
using Fingo.Auth.Domain.Projects.Interfaces;

namespace Fingo.Auth.Domain.Projects.Implementation
{
    public class AddProject : IAddProject
    {
        private readonly IEventBus _eventBus;
        private readonly IProjectRepository _repo;

        public AddProject(IProjectRepository repo , IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }

        public void Invoke(ProjectModel projectModel)
        {
            if (string.IsNullOrEmpty(projectModel.Name))
                throw new ArgumentNullException($"Cannot add new project with empty name.");
            var project = new Project {Name = projectModel.Name};
            _repo.Add(project);

            _eventBus.Publish(new ProjectAdded(project.Id , project.Name));
        }
    }
}