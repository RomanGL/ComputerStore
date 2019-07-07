using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Models
{
    public sealed class Product
    {
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a decription")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a product price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
    }
}
