using System;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Bases
{
    public abstract class AuditEntityBase : EntityBase
    {
        public DateTime? CreateDate { get; set; }
        public Guid? CreateByUserID { get; set; }
        public User CreateByUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateByUserID { get; set; }
        public User UpdateByUser { get; set; }
    }
}
