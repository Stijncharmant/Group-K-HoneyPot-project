namespace API.Models.Part
{
    public class PartSummaryDto
    {
        public int Id { get; set; }
        public int Sku { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
