using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartsInventoryWebApp.Models;
using System.Text;
using System.Text.Json;

namespace PartsInventoryWebApp.Pages
{
    [Authorize]
    public class InventoryModel : PageModel
    {
        // private fields
        private readonly IHttpClientFactory _httpClientFactory;

        // properties
        public List<PartSummaryDto> Parts { get; set; } = new();


        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; } = 5;

        [BindProperty]
        public PartCreateDto NewPart { get; set; } = new PartCreateDto();

        [BindProperty]
        public int EditPartId { get; set; }

        // constructor
        public InventoryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // methods
        public async Task OnGetAsync()
        {
            await LoadPartsFromApiAsync("https://localhost:7294/Parts");
        }

        public async Task<IActionResult> OnPostAddPartAsync()
        {

            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                await LoadPartsFromApiAsync("https://localhost:7294/Parts");

                ViewData["KeepModalOpen"] = true;
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            string apiUrl = "https://localhost:7294/Parts";

            var jsonPayload = new StringContent(
                JsonSerializer.Serialize(NewPart),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await client.PostAsync(apiUrl, jsonPayload);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Inventory");
                }

                ModelState.AddModelError(string.Empty, "The server rejected the data.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Unable to communicate with the backend service API.");
            }

            await LoadPartsFromApiAsync("https://localhost:7294/Parts");
            ViewData["KeepModalOpen"] = true;
            return Page();
        }

        public async Task<IActionResult> OnGetPartDetailsAsync(int id)
        {
            if (!User.IsInRole("Admin")) return Forbid();

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7294/Parts/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return Content(json, "application/json");
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostEditPartAsync()
        {
            if (!User.IsInRole("Admin")) return Forbid();

            if (!ModelState.IsValid)
            {
                await LoadPartsFromApiAsync("https://localhost:7294/Parts");
                ViewData["KeepEditModalOpen"] = true;
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            string apiUrl = $"https://localhost:7294/Parts/{EditPartId}";

            var jsonPayload = new StringContent(
                JsonSerializer.Serialize(NewPart),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                // Sending custom secure role header along with the PUT request
                var request = new HttpRequestMessage(HttpMethod.Put, apiUrl) { Content = jsonPayload };
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "User";
                request.Headers.Add("X-Employee-Role", userRole);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Inventory");
                }

                ModelState.AddModelError(string.Empty, "The server rejected the update parameters.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Unable to communicate with the backend service API.");
            }

            await LoadPartsFromApiAsync("https://localhost:7294/Parts");
            ViewData["KeepEditModalOpen"] = true;
            return Page();
        }

        public async Task<IActionResult> OnPostDeletePartAsync(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid part identification token provided.");
                await LoadPartsFromApiAsync("https://localhost:7294/Parts");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            string apiUrl = $"https://localhost:7294/Parts/{id}";

            try
            {
                // Execute the backend soft-delete connection
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Inventory");
                }

                ModelState.AddModelError(string.Empty, "The system backend rejected the deletion sequence.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Unable to communicate with the core API system service.");
            }

            // Fallback reload if something blocks execution
            await LoadPartsFromApiAsync("https://localhost:7294/Parts");
            return Page();
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index");
        }

        private async Task LoadPartsFromApiAsync(string apiUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Parts = JsonSerializer.Deserialize<List<PartSummaryDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PartSummaryDto>();
            }
        }
    }
}
