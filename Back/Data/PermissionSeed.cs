using Back.Models;

namespace Back.Data.Seed
{
    public static class PermissionSeed
    {
        public static class Users
        {
            public const string View = "USERS_VIEW";
            public const string Create = "USERS_CREATE";
            public const string Update = "USERS_UPDATE";
            public const string Delete = "USERS_DELETE";
        }

        public static List<Permission> Get()
        {
            return new()
            {
                new Permission { Code = Users.View, Description = "Ver usuarios" },
                new Permission { Code = Users.Create, Description = "Crear usuarios" },
                new Permission { Code = Users.Update, Description = "Actualizar usuarios" },
                new Permission { Code = Users.Delete, Description = "Eliminar usuarios" }
            };
        }
    }
}
