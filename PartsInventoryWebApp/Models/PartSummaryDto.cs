namespace PartsInventoryWebApp.Models
{
    public class PartSummaryDto
    {
        public int Id { get; set; }

        public int Sku { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
