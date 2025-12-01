using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace SecurityCapstone.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }


        public bool ShowSanitized { get; set; }

        public void OnGet()
        {
            ShowSanitized = false;
        }
        public void OnPost()
        {
            Username = SanitizeInput(Username);
            Email = SanitizeInput(Email);
            ShowSanitized = true;
            // Proceed with further validation or processing
        }

        static public string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Remove script tags and HTML
            input = Regex.Replace(input, "<.*?>", string.Empty);

            // Remove SQL meta-characters
            input = Regex.Replace(input, @"['\"";\-]", string.Empty);

            // Optionally, remove any non-alphanumeric characters except @ and .
            input = Regex.Replace(input, @"[^a-zA-Z0-9@.\-_]", string.Empty);

            return input.Trim();
        }
    }
}