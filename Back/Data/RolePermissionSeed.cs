using Back.Models;

namespace Back.Data.Seed
{
    public static class RolePermissionSeed
    {
        public static List<RolePermission> Get(
            Role adminRole,
            List<Permission> permissions)
        {
            return permissions.Select(p => new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = p.Id
            }).ToList();
        }
    }
}
