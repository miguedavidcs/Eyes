using System.ComponentModel.DataAnnotations;
using Back.Models;

namespace Back.Models
{
   public class User : BaseEntity
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(25)]
    [RegularExpression(@"^\d+$", ErrorMessage = "El numero debe contener solo digitos.")]
    public string Phone { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

}