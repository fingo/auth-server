using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Policies.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.Policies.Services.Interfaces;

namespace Fingo.Auth.Domain.Policies.Factories.Actions.Implementation
{
    public class GetPoliciesFromProject : IGetPoliciesFromProject
    {
        private readonly IProjectRepository projectRepository;
        private readonly IPolicyJsonConvertService jsonConvertService;

        public GetPoliciesFromProject(IProjectRepository projectRepository, IPolicyJsonConvertService jsonConvertService)
        {
            this.projectRepository = projectRepository;
            this.jsonConvertService = jsonConvertService;
        }

        public List<Tuple<Policy, PolicyConfiguration>> Invoke(int projectId)
        {
            var project = projectRepository.GetById(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            try
            {
                return project.ProjectPolicies
                    .Select(pp => new Tuple<Policy, PolicyConfiguration>
                        (pp.Policy, jsonConvertService.Deserialize(pp.Policy, pp.SerializedProjectPolicySetting)))
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"There was a problem with deserializing policy configurations of project with id: {projectId}, " +
                                    $"exception message: {e.Message}.");
            }
        }
    }
}