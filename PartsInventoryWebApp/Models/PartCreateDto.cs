using System.ComponentModel.DataAnnotations;

namespace PartsInventoryWebApp.Models
{
    public class PartCreateDto
    {
        [Required(ErrorMessage = "SKU is required.")]
        public int? Sku { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be greater than 0.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, 100000, ErrorMessage = "Quantity cannot be negative.")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Manufacturer ID is required.")]
        public int? ManufacturerId { get; set; }
    }
}
