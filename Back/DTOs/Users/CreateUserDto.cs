using System.ComponentModel.DataAnnotations;

namespace Back.DTOs.Users
{
    public class CreateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Required]
        [MaxLength(15)]
        [RegularExpression(@"^\+?[0-9]{7,15}$",
        ErrorMessage = "El teléfono debe contener solo números y tener entre 7 y 15 dígitos")]
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public List<string> Roles { get; set; } =[];
    }
    
}