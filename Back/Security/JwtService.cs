using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Back.Models;
using Back.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Back.Security
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            User user,
            IEnumerable<string> roles,
            IEnumerable<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullName", user.FullName)
            };

            // 🔑 ROLES (Admin, User, etc.)
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 🔐 PERMISOS (si los sigues usando)
            foreach (var permission in permissions)
            {
                claims.Add(new Claim(CustomClaimTypes.Permission, permission));
            }

            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is not configured.");

            var securityKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(key));

            var creds = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var expirationHours = int.Parse(
                _configuration["Jwt:ExpirationHours"] ?? "24");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expirationHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
