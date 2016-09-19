using Fingo.Auth.DbAccess.Models.Base;

namespace Fingo.Auth.DbAccess.Models.Policies
{
    public class UserPolicies:BaseEntity
    {
        public string SerializedUserPolicySetting { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProjectPoliciesId { get; set; }
        public ProjectPolicies ProjectPolicies { get; set; }
    }
}