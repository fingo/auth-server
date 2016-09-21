using Fingo.Auth.DbAccess.Models.Statuses;

namespace Fingo.Auth.DbAccess.Models.Base
{
    public class BaseEntityWithUserStatus : BaseEntity
    {
        public BaseEntityWithUserStatus()
        {
            Status = UserStatus.Registered;
        }

        public UserStatus Status { get; set; }
    }
}