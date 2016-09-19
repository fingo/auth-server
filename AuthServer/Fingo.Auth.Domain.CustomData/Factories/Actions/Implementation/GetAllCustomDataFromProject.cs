using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class GetAllCustomDataFromProject : IGetAllCustomDataFromProject
    {
        private readonly IProjectRepository _projectRepository;

        public GetAllCustomDataFromProject(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public List<ProjectCustomData> Invoke(int projectId)
        {
            var project = _projectRepository.GetByIdWithCustomDatas(projectId);
            if (project == null)
                throw new Exception($"Could not find project with ID: {projectId}.");

            return project.ProjectCustomData;
        }
    }
}