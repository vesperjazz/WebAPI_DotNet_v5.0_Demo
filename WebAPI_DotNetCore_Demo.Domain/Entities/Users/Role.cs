using System.Collections.Generic;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Users
{
    public class Role : EntityBase
    {
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
