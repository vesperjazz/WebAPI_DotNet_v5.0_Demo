using System;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Users
{
    public class UserRole : EntityBase
    {
        public Guid? UserID { get; set; }
        public User User { get; set; }

        public Guid? RoleID { get; set; }
        public Role Role { get; set; }
    }
}
