using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.Domain.Models.ProjectModels
{
    public class ProjectDetailWithUsersModel : ProjectDetailModel
    {
        public ProjectDetailWithUsersModel()
        {
            Users = new List<BaseUserModel>();
        }

        public ProjectDetailWithUsersModel(Project project,IEnumerable<User> users ) : base(project)
        {
            Users=new List<BaseUserModel>();
            foreach(var user in users)
            {
                Users.Add(new BaseUserModel(user));
            }
        }
        public List<BaseUserModel> Users { get; set; }
    }
}