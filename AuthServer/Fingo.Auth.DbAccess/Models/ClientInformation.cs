using System.ComponentModel.DataAnnotations;
using Fingo.Auth.DbAccess.Models.Base;

namespace Fingo.Auth.DbAccess.Models
{
    public class ClientInformation : BaseEntity
    {
        [MaxLength(100, ErrorMessage = "Too long name")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Too long contact data")]
        public string ContactData { get; set; }

        public int ProjectIdFk { get; set; }
        //[ForeignKey( "ProjectId" )]
        public virtual Project Project { get; set; }
    }
}