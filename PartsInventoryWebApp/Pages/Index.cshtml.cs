using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PartsInventoryWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            // Temporary test login
            if (Username == "admin" && Password == "admin")
            {
                return RedirectToPage("/Inventory");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return Page();
        }
    }
}
