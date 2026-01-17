using Microsoft.AspNetCore.Authorization;
using Back.Security.Claims;

namespace Back.Security.Permissions
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // 🔓 Bypass total para SuperAdmin
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // 🔐 Buscar permisos en los claims del JWT
            var permissions = context.User
                .FindAll(CustomClaimTypes.Permission)
                .Select(c => c.Value);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
