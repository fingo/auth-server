using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Models.UserModels
{
    public class BaseUserModelWithProjects : BaseUserModel
    {
        public BaseUserModelWithProjects(User user , IEnumerable<Project> projects) : base(user)
        {
            Projects = new List<ProjectModel>();
            foreach (var project in projects)
                Projects.Add(new ProjectModel(project));
        }

        public List<ProjectModel> Projects { get; set; }
    }
}