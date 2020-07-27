using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence;

namespace WebAPI_DotNetCore_Demo.Authorizations
{
    public class WebAPIDemoAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
    {
        private readonly IRepositoryContainer _repositoryContainer;
        public WebAPIDemoAuthorizationHandler(IRepositoryContainer repositoryContainer)
        {
            _repositoryContainer = repositoryContainer ?? throw new ArgumentNullException(nameof(repositoryContainer));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
        {
            var isMeetRoleRequirement = context.User
                .FindAll(requirement.ClaimType)
                .Any(c => c.Value == requirement.ClaimValue);

            if (isMeetRoleRequirement)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
