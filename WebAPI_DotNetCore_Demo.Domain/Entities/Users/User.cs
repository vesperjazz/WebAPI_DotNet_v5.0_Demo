using System.Collections.Generic;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Domain.Entities.Users
{
    public class User : AuditEntityBase
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsActive { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<User> CreateByUsers { get; set; }
        public ICollection<User> UpdateByUsers { get; set; }

        public ICollection<Person> CreateByPersons { get; set; }
        public ICollection<Person> UpdateByPersons { get; set; }
    }
}
