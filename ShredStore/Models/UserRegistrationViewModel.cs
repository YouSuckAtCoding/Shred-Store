using System.ComponentModel.DataAnnotations;

namespace ShredStore.Models
{
    public class UserRegistrationViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3,
           ErrorMessage = "Please meet the requirement (3 - 50 characters)")]
        public string Name { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3,
           ErrorMessage = "Please meet the requirement (3 - 50 characters)")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
