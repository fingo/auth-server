using System;
using System.Linq;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.CustomData;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class DeleteCustomDataFromProject : IDeleteCustomDataFromProject
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEventBus _eventBus;

        public DeleteCustomDataFromProject(IProjectRepository projectRepository, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _projectRepository = projectRepository;
        }

        public void Invoke(int projectId, string configurationName)
        {
            var project = _projectRepository.GetByIdWithCustomDatas(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var customData = project.ProjectCustomData.FirstOrDefault(data => data.ConfigurationName == configurationName);
            var removed = customData != null && project.ProjectCustomData.Remove(customData);

            if (!removed)
                throw new Exception($"Could not find custom data (name: {configurationName}) in project (id: {projectId}).");

            _projectRepository.Edit(project);
            _eventBus.Publish(new CustomDataRemoved(projectId, configurationName));
        }
    }
}