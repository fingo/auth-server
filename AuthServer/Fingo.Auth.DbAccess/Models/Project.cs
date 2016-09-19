using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.Policies;

namespace Fingo.Auth.DbAccess.Models
{
    public class Project : BaseEntityWithProjectStatus
    {
        public Project()
        {
            ProjectGuid = Guid.NewGuid();
            ProjectPolicies = new List<ProjectPolicies>();
            ProjectCustomData = new List<ProjectCustomData>();
            Information = new ClientInformation();
        }

        public Guid ProjectGuid { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        [MaxLength(30)]
        public string Name { get; set; }

        public virtual ClientInformation Information { get; set; }
        public virtual List<ProjectUser> ProjectUsers { get; set; }
        public virtual List<ProjectPolicies> ProjectPolicies { get; set; }
        public virtual List<ProjectCustomData> ProjectCustomData { get; set; }
    }
}