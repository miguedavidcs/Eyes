using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Back.Service.Interfaces;
using Back.DTOs.Users;
using Back.DTOs.Admin;

namespace Back.Controllers
{
    /// <summary>
    /// CASO DE USO 01: Gestión de Usuarios
    /// 
    /// Permite al administrador:
    /// - Ver usuarios
    /// - Crear usuarios
    /// - Asignar roles
    /// - Eliminar usuarios
    /// 
    /// La seguridad se controla EXCLUSIVAMENTE por policies.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Authorize] // JWT obligatorio para cualquier acción
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // =====================================================
        // CASO DE USO 01.1
        // Listar todos los usuarios
        // Permiso requerido: USERS_VIEW
        // =====================================================
        [Authorize(Policy = "USERS_VIEW")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // =====================================================
        // CASO DE USO 01.2
        // Obtener un usuario por ID
        // Permiso requerido: USERS_VIEW
        // =====================================================
        [Authorize(Policy = "USERS_VIEW")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // =====================================================
        // CASO DE USO 01.3
        // Crear un nuevo usuario
        // Permiso requerido: USERS_CREATE
        // =====================================================
        [Authorize(Policy = "USERS_CREATE")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = await _userService.CreateAsync(dto);
            return Ok(user);
        }

        // =====================================================
        // CASO DE USO 01.4
        // Asignar roles a un usuario
        // Permiso requerido: USERS_UPDATE
        // =====================================================
        [Authorize(Policy = "USERS_UPDATE")]
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateRoles(
            Guid id,
            [FromBody] UpdateRolesDto dto)
        {
            await _userService.UpdateRolesAsync(id, dto.Roles);
            return NoContent();
        }

        // =====================================================
        // CASO DE USO 01.5
        // Eliminar (soft delete) un usuario
        // Permiso requerido: USERS_DELETE
        // =====================================================
        [Authorize(Policy = "USERS_DELETE")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
