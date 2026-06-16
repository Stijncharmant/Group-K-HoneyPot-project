namespace API.Models.Part
{
    public class PartDetailsDto
    {
        public required int Id { get; set; }
        public int Sku { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public short Year { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
    }
}
