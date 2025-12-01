using System.ComponentModel.DataAnnotations;

namespace Api.Domain
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MinLength(2), StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }
}
