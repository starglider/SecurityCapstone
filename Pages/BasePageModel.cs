using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCapstone.Services;

namespace SecurityCapstone.Pages
{
    public class BasePageModel : PageModel
    {
        protected readonly AuthService? _authService;

        public BasePageModel()
        {
        }

        public BasePageModel(AuthService authService)
        {
            _authService = authService;
        }

        public bool IsAuthenticated => _authService?.IsAuthenticated(HttpContext) ?? false;
        public string? CurrentUsername => _authService?.GetUsername(HttpContext);
        public string? CurrentRole => _authService?.GetRole(HttpContext);
    }
}
