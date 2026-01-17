using Back.Data;
using Back.Models;
using Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Back.Repositories
{
    public class UserRepository: IUserRepository
    {
        // Implementation of user-related data operations
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Este Metodo por medio del email obtiene el usuario incluyendo sus roles y verifica que no este eliminado
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
        // Este Metodo por medio del Id obtiene el usuario incluyendo sus roles

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }
        // Este Metodo obtiene todos los usuarios que no estan eliminados
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u=> !u.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
        }
        // Este Metodo agrega un nuevo usuario a la base de datos
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        // Este Metodo actuliza un usuario existente en la base de datos con fecha de actualizacion
        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
        // Este Metodo elimina un usuario de la base de datos por su Id
        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return;
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}