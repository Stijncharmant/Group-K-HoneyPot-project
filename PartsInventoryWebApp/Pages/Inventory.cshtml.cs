using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PartsInventoryWebApp.Models;
using System.Text.Json;

namespace PartsInventoryWebApp.Pages
{
    public class InventoryModel : PageModel
    {
        // private fields
        private readonly IHttpClientFactory _httpClientFactory;

        // properties
        public List<PartSummaryDto> Parts { get; set; } = new();


        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; } = 5;

        // constructor
        public InventoryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // methods
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            string apiUrl = "https://localhost:7294/Parts";

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                Parts = JsonSerializer.Deserialize<List<PartSummaryDto>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<PartSummaryDto>();
            }
        }
    }
}
