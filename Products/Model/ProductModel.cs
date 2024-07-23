using System.ComponentModel.DataAnnotations;

namespace Products.Model
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; } = string.Empty;
        public string? CategoryId { get; set; } = string.Empty;
        public string? CategoryName { get; set; } = string.Empty;
        public string? Design { get; set; } = string.Empty;
        public string? Size { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
