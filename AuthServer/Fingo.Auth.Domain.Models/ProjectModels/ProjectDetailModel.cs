using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models;

namespace Fingo.Auth.Domain.Models.ProjectModels
{
    public class ProjectDetailModel : ProjectModel
    {
        public ProjectDetailModel()
        {
            
        }

        public ProjectDetailModel(Project project) : base(project)
        {
            InformationName = project.Information?.Name;
            ContactData = project.Information?.ContactData;
        }

        [MaxLength(100, ErrorMessage = "Too long name")]
        public string InformationName { get; set; }

        [MaxLength(200, ErrorMessage = "Too long contact data")]
        public string ContactData { get; set; }
    }
}