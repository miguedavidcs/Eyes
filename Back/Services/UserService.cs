using AutoMapper;
using Back.Data;
using Back.DTOs.Users;
using Back.Models;
using Back.Repositories.Interfaces;
using Back.Security.CurrentUser;
using Back.Service.Interfaces;

namespace Back.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly ICurrentUserContext _currentUserContext;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            AppDbContext context,
            ICurrentUserContext currentUserContext)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserContext = currentUserContext ?? throw new ArgumentNullException(nameof(currentUserContext));
        }

        // =========================
        // CREATE
        // =========================
        public async Task<UserReponseDTO> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            user.IsDeleted = false;

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserReponseDTO>(user);
        }

        // =========================
        // GET ALL (ADMIN)
        // =========================
        public async Task<IEnumerable<UserReponseDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserReponseDTO>>(users);
        }

        // =========================
        // GET BY ID (ADMIN / SELF)
        // =========================
        public async Task<UserReponseDTO?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserReponseDTO>(user);
        }

        // =========================
        // UPDATE (SIN PASSWORD)
        // =========================
        public async Task UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id)
                ?? throw new Exception("Usuario no encontrado");

            _mapper.Map(dto, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId)
                ?? throw new Exception("Usuario no encontrado");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                throw new Exception("Contraseña actual incorrecta");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================
        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new Exception("Usuario no encontrado");

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // =========================
        // UPDATE ROLES (ADMIN)
        // =========================
        public async Task UpdateRolesAsync(Guid targetUserId, List<string> roles)
        {
            var currentUserId = _currentUserContext.UserId;

            var user = await _userRepository.GetByIdAsync(targetUserId)
                ?? throw new Exception("Usuario no encontrado");

            // 🔐 Regla crítica: no permitir auto-desadmin
            var isSelf = currentUserId == targetUserId;
            var isRemovingAdmin = !roles.Contains("Admin");

            if (isSelf && isRemovingAdmin)
                throw new Exception("No puedes quitarte tu propio rol de administrador");

            // Limpiar roles actuales
            if (user.UserRoles.Any())
                _context.UserRoles.RemoveRange(user.UserRoles);

            var dbRoles = await _roleRepository.GetByNamesAsync(roles);
            if (!dbRoles.Any())
                throw new Exception("Roles inválidos");

            foreach (var role in dbRoles)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                });
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
