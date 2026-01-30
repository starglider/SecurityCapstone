using Microsoft.EntityFrameworkCore;
using SecurityCapstone.Data;

namespace SecurityCapstone.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private const string AuthCookieName = "AuthToken";
        private const string RoleCookieName = "UserRole";
        private const string UsernameCookieName = "Username";

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> ValidateCredentials(string username, string password)
        {
            return await _db.Users
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();
        }

        public void SetAuthCookie(HttpContext context, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                SameSite = SameSiteMode.Strict
            };

            // Store a simple token (in production, use a proper token)
            context.Response.Cookies.Append(AuthCookieName, $"{user.UserID}_{user.Username}", cookieOptions);
            context.Response.Cookies.Append(RoleCookieName, user.Role, cookieOptions);
            context.Response.Cookies.Append(UsernameCookieName, user.Username, cookieOptions);
        }

        public void ClearAuthCookie(HttpContext context)
        {
            context.Response.Cookies.Delete(AuthCookieName);
            context.Response.Cookies.Delete(RoleCookieName);
            context.Response.Cookies.Delete(UsernameCookieName);
        }

        public bool IsAuthenticated(HttpContext context)
        {
            return context.Request.Cookies.ContainsKey(AuthCookieName);
        }

        public string? GetUsername(HttpContext context)
        {
            return context.Request.Cookies[UsernameCookieName];
        }

        public string? GetRole(HttpContext context)
        {
            return context.Request.Cookies[RoleCookieName];
        }
    }
}
