using Fingo.Auth.DbAccess.Models.Base;

namespace Fingo.Auth.DbAccess.Models.CustomData
{
    public class UserCustomData : BaseEntity
    {
        public string SerializedConfiguration { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProjectCustomDataId { get; set; }
        public ProjectCustomData ProjectCustomData { get; set; }
    }
}