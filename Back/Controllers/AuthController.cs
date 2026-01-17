using Back.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [ApiController] 
    [Route("api/auth")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.RegisterAsync(
                    request.FullName,
                    request.Email,
                    request.Password
                );
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) // 🔴 ESTO TAMBIÉN
        {
            try
            {
                var token = await _authService.LoginAsync(
                    request.Email,
                    request.Password
                );
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }

    public record RegisterRequest(
        string FullName,
        string Email,
        string Password
    );

    public record LoginRequest(
        string Email,
        string Password
    );
}
