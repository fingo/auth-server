using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Models.CustomData;
using Fingo.Auth.DbAccess.Models.Policies.Enums;

namespace Fingo.Auth.Domain.Models.ProjectModels
{
    public class ProjectDetailWithAll : ProjectDetailWithUsersModel
    {
        public ProjectDetailWithAll()
        {
            Data = new List<ProjectCustomData>();
            Policies = new List<Policy>();
        }

        public ProjectDetailWithAll(Project project , IEnumerable<User> users , IEnumerable<ProjectCustomData> data ,
            IEnumerable<Policy> policies) : base(project , users)
        {
            Data = new List<ProjectCustomData>();
            foreach (var dataPiece in data)
                Data.Add(dataPiece);
            Policies = new List<Policy>();
            foreach (var policy in policies)
                Policies.Add(policy);
        }

        public List<ProjectCustomData> Data { get; set; }
        public List<Policy> Policies { get; set; }
    }
}