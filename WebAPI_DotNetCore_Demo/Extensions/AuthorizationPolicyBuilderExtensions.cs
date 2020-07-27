using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebAPI_DotNetCore_Demo.Authorizations;

namespace WebAPI_DotNetCore_Demo.Extensions
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireRoleAuthorization(
            this AuthorizationPolicyBuilder authorizationPolicyBuilder, string roleClaim)
        {
            authorizationPolicyBuilder.AddRequirements(
                new RoleAuthorizationRequirement(ClaimTypes.Role, roleClaim));

            return authorizationPolicyBuilder;
        }
    }
}
