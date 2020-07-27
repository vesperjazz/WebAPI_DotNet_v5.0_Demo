using Microsoft.AspNetCore.Authorization;

namespace WebAPI_DotNetCore_Demo.Authorizations
{
    public class RoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }
        public RoleAuthorizationRequirement(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
