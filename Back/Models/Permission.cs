using System.ComponentModel.DataAnnotations;

namespace Back.Models
{
    public class Permission : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Code {get; set;} = null!;
        [Required]
        [MaxLength(255)]
        public string Description {get; set;} = null!;
        public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();
    }
}
