using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityCapstone.Data;
using System.Text.RegularExpressions;

namespace SecurityCapstone.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = ""; 

        [BindProperty]
        public string Email { get; set; } = "";

        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public User? LoggedUser { get; private set; }

        public bool ShowSanitized { get; set; }

        // Add this property to hold all users
        public List<User> Users { get; private set; } = new();

        public void OnGet()
        {
            ShowSanitized = false;
            // Populate Users with all users from the database
            Users = _db.Users.ToList();
        }

        public async Task OnPost()
        {
            Username = SanitizeInput(Username);
            Email = SanitizeInput(Email);
            ShowSanitized = true;
            LoggedUser = await _db.Users
                .Where(u => u.Username == Username & u.Email == Email)
                .FirstOrDefaultAsync();

            // Populate Users with all users from the database asynchronously
            Users = await _db.Users.ToListAsync();
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