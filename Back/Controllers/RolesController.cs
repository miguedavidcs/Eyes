using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Back.Repositories.Interfaces;
using Back.DTOs.Roles;
using Back.Models;

namespace Back.Controllers
{
    /// <summary>
    /// CASO DE USO 02: Gestión de Roles
    ///
    /// Permite al administrador:
    /// - Ver roles
    /// - Crear roles
    /// - Modificar roles
    /// - Eliminar roles
    ///
    /// La autorización se controla únicamente por policies.
    /// </summary>
    [ApiController]
    [Route("api/roles")]
    [Authorize] // JWT obligatorio
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // =====================================================
        // CASO DE USO 02.1
        // Listar todos los roles
        // Permiso requerido: ROLES_VIEW
        // =====================================================
        [Authorize(Policy = "ROLES_VIEW")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleRepository.GetAllAsync();

            var response = roles.Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name
            });

            return Ok(response);
        }

        // =====================================================
        // CASO DE USO 02.2
        // Obtener un rol por ID
        // Permiso requerido: ROLES_VIEW
        // =====================================================
        [Authorize(Policy = "ROLES_VIEW")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        // =====================================================
        // CASO DE USO 02.3
        // Crear un nuevo rol
        // Permiso requerido: ROLES_CREATE
        // =====================================================
        [Authorize(Policy = "ROLES_CREATE")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var name = dto.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("El nombre del rol es obligatorio");

            var exists = await _roleRepository.GetByNameAsync(name);
            if (exists != null)
                return Conflict("El rol ya existe");

            var role = new Role
            {
                Name = name,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _roleRepository.AddAsync(role);

            return Ok(new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        // =====================================================
        // CASO DE USO 02.4
        // Actualizar un rol
        // Permiso requerido: ROLES_UPDATE
        // =====================================================
        [Authorize(Policy = "ROLES_UPDATE")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID no coincide");

            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            var newName = dto.Name?.Trim();
            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest("El nombre del rol es obligatorio");

            var duplicate = await _roleRepository.GetByNameAsync(newName);
            if (duplicate != null && duplicate.Id != id)
                return Conflict("Ya existe un rol con ese nombre");

            role.Name = newName;
            role.UpdatedAt = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(role);

            return NoContent();
        }

        // =====================================================
        // CASO DE USO 02.5
        // Eliminar un rol (soft delete)
        // Permiso requerido: ROLES_DELETE
        // =====================================================
        [Authorize(Policy = "ROLES_DELETE")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            if (role.Name == "Admin")
                return BadRequest("No se puede eliminar el rol Administrador");

            role.IsDeleted = true;
            role.UpdatedAt = DateTime.UtcNow;

            await _roleRepository.DeleteAsync(role);

            return NoContent();
        }
    }
}
