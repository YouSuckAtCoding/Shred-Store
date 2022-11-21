using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShredStore.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Product Image")]
        public IFormFile ImageFile { get; set; }
    }
}
