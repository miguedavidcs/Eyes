using System.ComponentModel.DataAnnotations;

namespace Back.DTOs.Users
{
    public class UserUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(25)]
        [RegularExpression(@"^\d+$")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string? Password { get; set; }

        public bool IsActive { get; set; }
    }
}
