using System.ComponentModel.DataAnnotations;

namespace Back.DTOs.Roles
{
    public class CreateRoleDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set;} = string.Empty;
    }
}