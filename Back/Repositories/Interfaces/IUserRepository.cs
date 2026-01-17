using Back.Models;

namespace Back.Repositories.Interfaces
{
    public interface IUserRepository
    {
        // Method signatures for user-related data operations
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email);

    }
}