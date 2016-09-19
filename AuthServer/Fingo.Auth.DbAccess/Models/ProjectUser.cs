namespace Fingo.Auth.DbAccess.Models
{
    public class ProjectUser
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}