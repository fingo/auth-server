using Fingo.Auth.DbAccess.Models.Statuses;

namespace Fingo.Auth.DbAccess.Models.Base
{
    public class BaseEntityWithProjectStatus : BaseEntity
    {
        public BaseEntityWithProjectStatus()
        {
            Status = ProjectStatus.Active;
        }

        public ProjectStatus Status { get; set; }
    }
}