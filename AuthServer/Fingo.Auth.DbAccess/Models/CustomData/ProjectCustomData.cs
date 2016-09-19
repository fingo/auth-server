using System.Collections.Generic;

namespace Fingo.Auth.DbAccess.Models.CustomData
{
    public class ProjectCustomData : BaseCustomData
    {
        public ProjectCustomData()
        {
            UserCustomData = new List<UserCustomData>();
        }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public virtual List<UserCustomData> UserCustomData { get; set; }
    }
}