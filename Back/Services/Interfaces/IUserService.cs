using Back.DTOs.Users;
using Back.Models;

namespace Back.Service.Interfaces
{
    public interface IUserService
{
    Task<IEnumerable<UserReponseDTO>> GetAllAsync();
    Task<UserReponseDTO?> GetByIdAsync(Guid id);
    Task<UserReponseDTO> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(UserUpdateDto dto);
    Task DeleteAsync(Guid id);
    Task ChangePasswordAsync(ChangePasswordDto dto);
    Task UpdateRolesAsync(Guid userId, List<string> roles);
    
}

}