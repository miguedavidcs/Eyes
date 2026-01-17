using Back.Models;

namespace Back.Data.Seed
{
    public static class RoleSeed
    {
        public static readonly string SuperAdmin = "SuperAdmin";
        public static readonly string Admin = "Admin";
        public static readonly string User = "User";

        public static List<Role> Get()
        {
            return new()
            {
                new Role { Name = SuperAdmin },
                new Role { Name = Admin },
                new Role { Name = User }
            };
        }
    }
}
