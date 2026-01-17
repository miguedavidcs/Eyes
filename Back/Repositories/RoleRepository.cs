using Back.Data;
using Back.Models;
using Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Back.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL (sin eliminados)
        // =========================
        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<Role?> GetByIdAsync(Guid id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        // =========================
        // GET BY NAME
        // =========================
        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);
        }

        // =========================
        // GET BY NAMES (para asignación)
        // =========================
        public async Task<List<Role>> GetByNamesAsync(List<string> names)
        {
            return await _context.Roles
                .Where(r => names.Contains(r.Name) && !r.IsDeleted)
                .ToListAsync();
        }

        // =========================
        // CREATE
        // =========================
        public async Task AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================
        public async Task DeleteAsync(Role role)
        {
            role.IsDeleted = true;
            role.UpdatedAt = DateTime.UtcNow;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        // =========================
        // GET USER ROLES (para JWT)
        // =========================
        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive && !ur.IsDeleted)
                .Select(ur => ur.Role!.Name)
                .Distinct()
                .ToListAsync();
        }

        // =========================
        // GET USER PERMISSIONS (no usado en Fase 1, pero listo)
        // =========================
        public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive && !ur.IsDeleted)
                .SelectMany(ur => ur.Role!.RolePermissions)
                .Where(rp => rp.Permission.IsActive && !rp.Permission.IsDeleted)
                .Select(rp => rp.Permission.Code)
                .Distinct()
                .ToListAsync();
        }
    }
}
