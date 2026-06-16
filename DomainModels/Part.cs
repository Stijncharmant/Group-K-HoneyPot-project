namespace DomainModels
{
    public class Part
    {
        // Properties
        public int Id { get; private set; }
        public int Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public short Year { get; private set; }
        public int CategoryId { get; private set; }
        public int ManufacturerId { get; private set; }
        public bool IsArchived { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Part()
        {
        }

        // constructor
        public Part(
            int sku,
            string name,
            string description,
            decimal price,
            int quantity,
            short year,
            int categoryId,
            int manufacturerId)
        {
            Sku = sku;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            Year = year;
            CategoryId = categoryId;
            ManufacturerId = manufacturerId;

            IsArchived = false;
            CreatedAt = DateTime.UtcNow;
        }

        // domain methods
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Price cannot be negative.");

            Price = newPrice;
        }

        public void UpdateStock(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Quantity = newQuantity;
        }

        public void Archive()
        {
            IsArchived = true;
        }

        public void UpdateDetails(
            string name,
            string description,
            short year,
            int categoryId,
            int manufacturerId)
        {
            Name = name;
            Description = description;
            Year = year;
            CategoryId = categoryId;
            ManufacturerId = manufacturerId;
        }

    }
}
