using Back.Models;
using Back.Repositories.Interfaces;
using Back.Security;
using Back.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Back.Service
{
    public class AuthService : IAuthService
    {
        // Implementación de los métodos RegisterAsync y LoginAsync
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtService _jwtService;

        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(
                IUserRepository userRepository,
                IRoleRepository roleRepository,
                JwtService jwtService)
        {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _jwtService = jwtService;
        }

        public async Task RegisterAsync(string fullName, string email, string password)
        {
            var existing = await _userRepository.GetByEmailAsync(email);
            if (existing != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _userRepository.AddAsync(user);
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            // Verificar si el usuario existe
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new Exception("Credenciales Invalidas.");
            // Verificar la contraseña
            var result = _passwordHasher.VerifyHashedPassword(
                user, 
                user.PasswordHash, 
                password);
            // Si la verificación falla, lanzar una excepción
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Credenciales Invalidas");
            }
            // Generar y retornar el token JWT
            var roles = await _roleRepository.GetUserRolesAsync(user.Id);
            var permissions = await _roleRepository.GetUserPermissionsAsync(user.Id);
            return _jwtService.GenerateToken(user, roles, permissions);
        }
    }
}