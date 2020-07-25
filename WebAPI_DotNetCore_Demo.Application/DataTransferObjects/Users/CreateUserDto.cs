using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public List<Guid?> RoleIDs { get; set; }
    }
}
