using Back.Models;

namespace Back.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        // ===== Roles =====
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string name);
        Task<List<Role>> GetByNamesAsync(List<string> names); // ← ESTA LÍNEA FALTABA

        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);

        // ===== Usuarios / Permisos (ya existentes) =====
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task<List<string>> GetUserPermissionsAsync(Guid userId);
    }
}
