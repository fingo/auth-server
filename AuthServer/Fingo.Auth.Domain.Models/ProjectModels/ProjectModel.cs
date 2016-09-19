using System;
using Fingo.Auth.DbAccess.Models;

namespace Fingo.Auth.Domain.Models.ProjectModels
{
    public class ProjectModel
    {
        public ProjectModel()
        {
            
        }
        public ProjectModel(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            ProjectGuid = project.ProjectGuid;
            CreationDate = project.CreationDate;
            ModificationDate = project.ModificationDate;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Guid ProjectGuid { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }
    }
}