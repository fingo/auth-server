using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.Policies.Enums;

namespace Fingo.Auth.DbAccess.Models.Policies
{
    public class ProjectPolicies : BaseEntity
    {
        public ProjectPolicies()
        {
            UserPolicies = new List<UserPolicies>();
        }

        [Required]
        public Policy Policy { get; set; }
        public string SerializedProjectPolicySetting { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public virtual List<UserPolicies> UserPolicies { get; set; }
    }
}