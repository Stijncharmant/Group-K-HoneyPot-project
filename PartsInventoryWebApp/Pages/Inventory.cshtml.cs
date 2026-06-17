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
