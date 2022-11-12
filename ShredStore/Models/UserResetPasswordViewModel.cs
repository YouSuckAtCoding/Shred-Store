using System.ComponentModel.DataAnnotations;

namespace ShredStore.Models
{
    public class UserResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
