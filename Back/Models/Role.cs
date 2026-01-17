using System.ComponentModel.DataAnnotations;
using Back.Models;

namespace Back.Models
{
   public class Role : BaseEntity
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();
}

}