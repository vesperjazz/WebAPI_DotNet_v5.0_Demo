using System;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users
{
    public sealed class AuthenticatedUserDto
    {
        public DateTime? TokenExpiration { get; set; }
        public string AccessToken { get; set; }
    }
}
