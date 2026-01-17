using System.ComponentModel.DataAnnotations;

namespace Back.DTOs.Roles
{
    public class UpdateRoleDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
