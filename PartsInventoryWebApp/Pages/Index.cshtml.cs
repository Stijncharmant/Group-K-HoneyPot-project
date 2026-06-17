using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartsInventoryWebApp.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PartsInventoryWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginData = new { Email = Email, Password = Password };
            var jsonPayload = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            try
            {
                // Remember to verify this port matches your running API's port
                var response = await client.PostAsync("https://localhost:7294/Employees/login", jsonPayload);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "The combination of email and password was not found.");
                    return Page();
                }

                var responseString = await response.Content.ReadAsStringAsync();

                // 2. Safely deserialize straight into your clean new Frontend DTO
                var employee = JsonSerializer.Deserialize<EmployeeSummaryDto>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (employee == null)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred processing your login data.");
                    return Page();
                }

                // 3. Build session details from the DTO properties
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
                    new Claim(ClaimTypes.Email, employee.Email),
                    new Claim(ClaimTypes.Role, employee.IsAdmin ? "Admin" : "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Inventory");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Unable to communicate with the API server. Is your backend running?");
                return Page();
            }
        }
    }
}
