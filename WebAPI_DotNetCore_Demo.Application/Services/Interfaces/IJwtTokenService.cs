using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GetAccessToken(Guid UserID, string userName, IEnumerable<string> roles,
            string issuer, string audience, string secretKey, DateTime expirationDate);
    }
}
