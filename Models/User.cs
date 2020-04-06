using System.ComponentModel.DataAnnotations;

namespace FindbookApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Length must be between 1 and 50 characters")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordDigest { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
