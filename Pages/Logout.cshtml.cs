using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCapstone.Services;

namespace SecurityCapstone.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly AuthService _authService;

        public LogoutModel(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            _authService.ClearAuthCookie(HttpContext);
            return Page();
        }

        public IActionResult OnPost()
        {
            _authService.ClearAuthCookie(HttpContext);
            return RedirectToPage("/Logout");
        }
    }
}
