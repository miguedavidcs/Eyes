using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Back.Security.CurrentUser
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return userId != null
                    ? Guid.Parse(userId)
                    : Guid.Empty;
            }
        }

        public bool IsAdmin =>
            _httpContextAccessor.HttpContext?
                .User?
                .IsInRole("Admin") ?? false;
    }
}
