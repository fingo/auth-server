using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.Domain.Models.CustomData;

namespace Fingo.Auth.Domain.Models.ProjectModels
{
    public class ProjectWithCustomDataModel : ProjectModel
    {
        public ProjectWithCustomDataModel(Project project) : base(project)
        {
            ProjectCustomData = new List<ProjectCustomDataModel>();
            foreach (var projectCustomData in project.ProjectCustomData)
                ProjectCustomData.Add(new ProjectCustomDataModel(projectCustomData));
        }

        public List<ProjectCustomDataModel> ProjectCustomData { get; set; }
    }
}