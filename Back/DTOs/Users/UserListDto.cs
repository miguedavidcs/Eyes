namespace Back.DTOs.Users
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive {get; set; } = false;
        public List<string> Roles {get; set;} = [];
    }
}