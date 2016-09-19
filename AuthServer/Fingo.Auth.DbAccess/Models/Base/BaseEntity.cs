using System;
using System.ComponentModel.DataAnnotations;

namespace Fingo.Auth.DbAccess.Models.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreationDate = DateTime.UtcNow;
            ModificationDate = DateTime.UtcNow;
        }

        public int Id { get; set; }


        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy H:mm:ss}")]
        public DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy H:mm:ss}")]
        public DateTime ModificationDate { get; set; }
    }
}