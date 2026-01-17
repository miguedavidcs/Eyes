namespace Back.DTOs.Users
{
    public class UpdateUserRolesDto
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
