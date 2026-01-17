namespace Back.Service.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(string fullName, string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}