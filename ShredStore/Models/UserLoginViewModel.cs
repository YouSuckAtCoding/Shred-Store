using System.ComponentModel.DataAnnotations;

namespace ShredStore.Models
{
    public class UserLoginViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3,
           ErrorMessage = "Please met the requirement (3 - 50 characters)")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
